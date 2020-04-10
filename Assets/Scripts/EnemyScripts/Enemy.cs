using Panda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum TriggerType { Enter, Stay, Exit }

    public int health = 3;

    public EnemyHealthBar healthBar;

    /* Changing vision area / hearing area
     * 
     * Vision can be changed on the enemy's LineOfSight child in the heirarchy.
     * This child can be scaled to easily change distance/width. To change the angle
     * click "Edit Collider" on its Polygon Collider 2D component.
     * 
     * Hearing radius can be changed on the enemy's HearingRadius child in the heirarchy.
     * Changing the radius of the Circle Collider 2D component will change its area.
     */

    public float attackCooldown = 1.0f;
    public float idleSpeed = 1.0f;
    public float chaseSpeed = 2.1f;

    public GameObject lineOfSight;
    public GameObject losIndicator; //For demo purposes
    public GameObject hearingRadius;
    public GameObject hearIndicator; //For demo purposes
    public GameObject attackZone;

    public WaypointPath waypointPath;

    public GameObject spottedSign; // Appears when the player is spotted

    public MeshRenderer bodyMesh;

    private BasicMovement playerTarget;
    private bool isDead;
    private float nextAttackTime;
    private PolygonCollider2D losCollider;
    private bool canSeePlayer;
    private bool canHearPlayer;
    private int waypointIndex;
    private Vector3 waypointTarget;
    private Rigidbody2D rb;
    private bool hasAttacked;
    private bool isRunning;
    private bool isWalking;

    private SpriteRenderer sr;
    public bool playerSpotted;

    public float spottedIndex;

    public Animator animator;

    public GameObject player;

    public float noiseChaseMaxTime = 4.0f; // How long will the enemy try to find the noise
    private bool heardNoise;
    private Vector3 noiseLocation;
    private float noiseChaseTimeout;

    void Start()
    {
        player = GameObject.Find("Player-chan");

        playerTarget = null;
        nextAttackTime = 0.0f;
        canSeePlayer = false;
        canHearPlayer = false;
        waypointIndex = 0;
        waypointTarget = Vector3.zero;
        hasAttacked = false;

        playerSpotted = false;
        spottedIndex = 0;

        heardNoise = false;
        noiseChaseTimeout = 0.0f;

        EnemyChildDelegate losChild = lineOfSight.AddComponent<EnemyChildDelegate>();
        losChild.Parent = this;
        losChild.ChildTag = "LoS";

        EnemyChildDelegate earChild = hearingRadius.AddComponent<EnemyChildDelegate>();
        earChild.Parent = this;
        earChild.ChildTag = "Ear";

        EnemyChildDelegate atkChild = attackZone.AddComponent<EnemyChildDelegate>();
        atkChild.Parent = this;
        atkChild.ChildTag = "Atk";

        healthBar.MaxHealth = health;

        sr = GetComponent<SpriteRenderer>();
        losCollider = lineOfSight.GetComponent<PolygonCollider2D>();

        rb = GetComponent<Rigidbody2D>();

        if (!sr.flipX)
        {
            lineOfSight.transform.Rotate(0f, 0f, 180f);
            attackZone.transform.Rotate(0f, 0f, 180f);
        }
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (!hasAttacked)
            {
                if (playerTarget != null)
                {
                    animator.SetTrigger("Attack");
                    nextAttackTime = Time.time + attackCooldown;
                    hasAttacked = true;
                }
            }
            else
            {
                hasAttacked = false;
                animator.SetTrigger("AttackCooldownFinish");
            }
        }
        
        if (heardNoise)
        {
            if (Time.time >= noiseChaseTimeout)
            {
                heardNoise = false;
            }
        }

        manageSpotted();
        manageRange();
    }

    // Change range of sight and vision based on alert level
    void manageRange() {
        if (Alert_Level_None()) {
            lineOfSight.transform.localScale = new Vector3 (1.6f, 1, 1);
        } else if (Alert_Level_Low()) {
            lineOfSight.transform.localScale = new Vector3 (1.8f, 1, 1);
        } else if (Alert_Level_Medium()) {
            lineOfSight.transform.localScale = new Vector3 (2.0f, 1, 1);
        } else if (Alert_Level_High()) {
            lineOfSight.transform.localScale = new Vector3 (2.2f, 1, 1);
        } 
    }

    private void LateUpdate()
    {
        if (!isWalking && !isRunning)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
        else
        {
            if (isWalking)
            {
                isWalking = false;
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
            }
            if (isRunning)
            {
                isRunning = false;
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
            }
        }
    }

    void manageSpotted() {
        if (canSeePlayer || canHearPlayer) {
            spottedIndex += 2;
        } else {
            spottedIndex -= 2;
        }
        if (spottedIndex <= 0) {
            spottedIndex = 0;
        }
        if (spottedIndex >= 100) {
            spottedIndex = 100;
        }
        if (spottedIndex > 40) {
            playerSpotted = true;
            spottedSign.GetComponent<SpriteRenderer>().enabled = true;
            heardNoise = false;
            /*
            if (!sr.flipX && player.transform.position.x <= this.transform.position.x) {
                //sr.flipX = true;
                //lineOfSight.transform.Rotate(0f, 0f, 180f);
                //attackZone.transform.Rotate(0f, 0f, 180f);
                FaceLeft();
            } else if (sr.flipX && player.transform.position.x >= this.transform.position.x) {
                //sr.flipX = false;
                //lineOfSight.transform.Rotate(0f, 0f, 180f);
                //attackZone.transform.Rotate(0f, 0f, 180f);
                FaceRight();
            }
            */
        }
        if (spottedIndex < 5) {
            playerSpotted = false;
            spottedSign.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void Damage(int amount)
    {
        healthBar.removeHealth(amount);
        health -= amount;
        if (health <= 0)
        {
            Destroy(this.gameObject);
            // bodyMesh.material.color = Color.red;
            // losIndicator.SetActive(false);
            // hearIndicator.SetActive(false);
            // this.enabled = false;
            isDead = true;
        }
    }

    public void HeardSuspiciousNoise(Vector2 location)
    {
        heardNoise = true;
        noiseLocation = location;
        noiseChaseTimeout = Time.time + noiseChaseMaxTime;
    }

    public void JumpAttack()
    {
        if (playerTarget != null)
        {
            playerTarget.TakeDamage(10);
        }
    }

    public void CooldownFinished()
    {
        //hasAttacked = false;
    }

    private void FaceLeft()
    {
        sr.flipX = true;
        lineOfSight.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        attackZone.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void FaceRight()
    {
        sr.flipX = false;
        lineOfSight.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        attackZone.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
    }

    public void OnChildTrigger(string childTag, Collider2D collision, TriggerType triggerType)
    {
        if (isDead)
            return;

        switch (childTag)
        {
            case "LoS": // Line of sight triggered
                if (triggerType == TriggerType.Enter)
                    HandleVisionEnter(collision.gameObject);
                else if (triggerType == TriggerType.Stay)
                    HandleVisionStay(collision.gameObject);
                else
                    HandleVisionExit(collision.gameObject);
                break;

            case "Ear": // Hearing radius triggered
                if (triggerType == TriggerType.Enter)
                    HandleHearingEnter(collision.gameObject);
                else if (triggerType == TriggerType.Stay)
                    HandleHearingStay(collision.gameObject);
                else
                    HandleHearingExit(collision.gameObject);
                break;
            case "Atk": // Attack zone triggered
                if (triggerType == TriggerType.Enter)
                    playerTarget = collision.gameObject.GetComponent<BasicMovement>();
                else if (triggerType == TriggerType.Stay) { }
                else
                    playerTarget = null;
                break;

            default:
                throw new MissingReferenceException("Could not find case for child with tag " + childTag);
        }
    }

    private void HandleVisionEnter(GameObject other)
    {
        if (CanSeePlayer(other))
        {
            canSeePlayer = true;
            other.GetComponent<BasicMovement>().alertLevel.incDetectionCount();
            other.GetComponent<BasicMovement>().alertLevel.increaseAlertPercentage(10);

            losIndicator.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        else
        {
            losIndicator.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void HandleVisionStay(GameObject other)
    {
        if (CanSeePlayer(other))
        {
            if (!canSeePlayer)
            {
                canSeePlayer = true;
                other.GetComponent<BasicMovement>().alertLevel.incDetectionCount();
                other.GetComponent<BasicMovement>().alertLevel.increaseAlertPercentage(10);
            }

            losIndicator.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        else
        {
            if (canSeePlayer)
            {
                canSeePlayer = false;
                other.GetComponent<BasicMovement>().alertLevel.decDetectionCount();
            }

            losIndicator.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    //Casts a ray toward the top of the player and the bottom of the player. If either hits, player is seen.
    private bool CanSeePlayer(GameObject other)
    {
        //Controls how strict the rays are
        float xPadding = 0.0f;//0.075f;
        float yPadding = 0.05f;

        Vector2 scaledLosOrigin = losCollider.points[0] * lineOfSight.transform.localScale;
        Vector3 startPos = transform.position + new Vector3(scaledLosOrigin.x, scaledLosOrigin.y);

        Vector3 upperTarget = other.transform.position + Vector3.up * (other.GetComponent<SpriteRenderer>().bounds.extents.y - yPadding);
        Vector3 lowerTarget = other.transform.position + Vector3.down * (other.GetComponent<SpriteRenderer>().bounds.extents.y - yPadding);

        Vector3 upperRightTarget = upperTarget + Vector3.right * (other.GetComponent<BoxCollider2D>().bounds.extents.x - xPadding);
        Vector3 lowerRightTarget = lowerTarget + Vector3.right * (other.GetComponent<BoxCollider2D>().bounds.extents.x - xPadding);
        Vector3 upperLeftTarget = upperTarget + Vector3.left * (other.GetComponent<BoxCollider2D>().bounds.extents.x + xPadding);
        Vector3 lowerLeftTarget = lowerTarget + Vector3.left * (other.GetComponent<BoxCollider2D>().bounds.extents.x + xPadding);
        Vector3 centerRightTarget = other.transform.position + Vector3.right * (other.GetComponent<BoxCollider2D>().bounds.extents.x + xPadding);
        Vector3 centerLeftTarget = other.transform.position + Vector3.left * (other.GetComponent<BoxCollider2D>().bounds.extents.x + xPadding);


        // Used to see vision rays in the editor
        /*
        Debug.DrawRay(startPos, upperRightTarget - startPos, Color.green);
        Debug.DrawRay(startPos, upperLeftTarget - startPos, Color.green);
        Debug.DrawRay(startPos, lowerLeftTarget - startPos, Color.green);
        Debug.DrawRay(startPos, lowerRightTarget - startPos, Color.green);
        Debug.DrawRay(lowerTarget, transform.right, Color.red);
        Debug.DrawRay(upperTarget, transform.right, Color.red);
        */
        // Used to see the vision points on the player
        /*
        Debug.DrawRay(upperRightTarget, Vector3.right * 0.1f, Color.green, 1f);
        Debug.DrawRay(centerRightTarget, Vector3.right * 0.1f, Color.green, 1f);
        Debug.DrawRay(lowerRightTarget, Vector3.right * 0.1f, Color.green, 1f);
        Debug.DrawRay(upperLeftTarget, Vector3.left * 0.1f, Color.green, 1f);
        Debug.DrawRay(centerLeftTarget, Vector3.left * 0.1f, Color.green, 1f);
        Debug.DrawRay(lowerLeftTarget, Vector3.left * 0.1f, Color.green, 1f);
        */

        int layerMask = ~((1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Indicator")));
        bool hasHit = false;

        RaycastHit2D upperVisCheck = Physics2D.Raycast(upperTarget, transform.right, Vector2.Distance(transform.position, other.transform.position), layerMask);
        RaycastHit2D lowerVisCheck = Physics2D.Raycast(lowerTarget, transform.right, Vector2.Distance(transform.position, other.transform.position), layerMask);

        if (!upperVisCheck || upperVisCheck.collider == null || !losCollider.IsTouching(upperVisCheck.collider))
        {
            if (losCollider.OverlapPoint(upperRightTarget))
            {
                RaycastHit2D hit = Physics2D.Linecast(startPos, upperRightTarget, layerMask);
                hasHit = !hit || hit.collider == null || hit.collider.tag.Equals("Player");
            }
            if (!hasHit && losCollider.OverlapPoint(upperLeftTarget))
            {
                RaycastHit2D hit = Physics2D.Linecast(startPos, upperLeftTarget, layerMask);
                hasHit = !hit || hit.collider == null || hit.collider.tag.Equals("Player");
            }
        }

        if (!lowerVisCheck || lowerVisCheck.collider == null || !losCollider.IsTouching(lowerVisCheck.collider))
        {
            if (!hasHit && losCollider.OverlapPoint(lowerRightTarget))
            {
                RaycastHit2D hit = Physics2D.Linecast(startPos, lowerRightTarget, layerMask);
                hasHit = !hit || hit.collider == null || hit.collider.tag.Equals("Player");
            }
            if (!hasHit && losCollider.OverlapPoint(lowerLeftTarget))
            {
                RaycastHit2D hit = Physics2D.Linecast(startPos, lowerLeftTarget, layerMask);
                hasHit = !hit || hit.collider == null || hit.collider.tag.Equals("Player");
            }
        }

        if (!hasHit && losCollider.OverlapPoint(centerRightTarget))
        {
            RaycastHit2D hit = Physics2D.Linecast(startPos, centerRightTarget, layerMask);
            hasHit = !hit || hit.collider == null || hit.collider.tag.Equals("Player");
        }
        if (!hasHit && losCollider.OverlapPoint(centerLeftTarget))
        {
            RaycastHit2D hit = Physics2D.Linecast(startPos, centerLeftTarget, layerMask);
            hasHit = !hit || hit.collider == null || hit.collider.tag.Equals("Player");
        }

        return hasHit;
    }

    private void HandleVisionExit(GameObject other)
    {
        if (canSeePlayer)
        {
            canSeePlayer = false;
            other.GetComponent<BasicMovement>().alertLevel.decDetectionCount();
        }
        losIndicator.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void HandleHearingEnter(GameObject player)
    {
        if (!player.GetComponent<BasicMovement>().isSneaking && player.GetComponent<BasicMovement>().isMoving) {
            canHearPlayer = true;
            player.GetComponent<BasicMovement>().alertLevel.incDetectionCount();
            player.GetComponent<BasicMovement>().alertLevel.increaseAlertPercentage(5);
            hearIndicator.GetComponent<SpriteRenderer>().color = Color.magenta;
        }
    }

    private void HandleHearingStay(GameObject player)
    {
        if (!player.GetComponent<BasicMovement>().isSneaking && player.GetComponent<BasicMovement>().isMoving)
        {
            if (!canHearPlayer)
            {
                canHearPlayer = true;
                player.GetComponent<BasicMovement>().alertLevel.incDetectionCount();
                player.GetComponent<BasicMovement>().alertLevel.increaseAlertPercentage(5);
            }
            hearIndicator.GetComponent<SpriteRenderer>().color = Color.magenta;
        }
        else
        {
            if (canHearPlayer)
            {
                canHearPlayer = false;
                player.GetComponent<BasicMovement>().alertLevel.decDetectionCount();
            }
            hearIndicator.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void HandleHearingExit(GameObject other)
    {
        if (canHearPlayer)
        {
            canHearPlayer = false;
            other.GetComponent<BasicMovement>().alertLevel.decDetectionCount();
        }
        hearIndicator.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public SpriteRenderer SpriteRenderer
    {
        get { return sr; }
    }

    [Task]
    public bool Alert_Level_None()
    {
        return AlertLevel.currentAlertPercentage == 0;
    }

    [Task]
    public bool Alert_Level_Low()
    {
        return AlertLevel.currentAlertPercentage < 31 && AlertLevel.currentAlertPercentage > 0;
    }

    [Task]
    public bool Alert_Level_Medium()
    {
        return AlertLevel.currentAlertPercentage > 30 && AlertLevel.currentAlertPercentage < 71;
    }

    [Task]
    public bool Alert_Level_High()
    {
        return AlertLevel.currentAlertPercentage > 70;
    }

    [Task]
    public bool PlayerSpotted()
    {
        return playerSpotted;
    }

    [Task]
    public bool IsAttacking()
    {
        return hasAttacked;
    }

    [Task]
    public bool CanSeePlayer()
    {
        return canSeePlayer;
    }

    [Task]
    public bool HeardNoise()
    {
        // Debug.Log("Heard noise is " + heardNoise);
        return heardNoise;
    }

    [Task]
    public void MoveTo_Noise()
    {
        if (Vector2.Distance(transform.position, noiseLocation) <= 0.3f)
        {
            Task.current.Succeed();
        }
        else
        {
            Vector3 dir = Vector3.Normalize(noiseLocation - transform.position);
            rb.MovePosition(transform.position + dir * Time.deltaTime * idleSpeed);
            isRunning = false;
            isWalking = true;
        }
        //WaitArrival(noiseLocation);
    }

    [Task]
    public bool Forget_Noise()
    {
        heardNoise = false;
        return true;
    }

    [Task]
    bool SetDestination_Waypoint()
    {
        bool isSet = false;
        if (waypointPath != null)
        {
            var i = waypointArrayIndex;
            waypointTarget = waypointPath.waypoints[i].position;
            waypointTarget.z = transform.position.z;
            isSet = true;
        }
        return isSet;
    }

    [Task]
    bool FlipToward_Target()
    {
        bool isSet = false;
        if (waypointTarget != null)
        {
            FlipTowardPosition(waypointTarget);
            isSet = true;
        }
        return isSet;
    }

    [Task]
    bool FlipToward_Noise()
    {
        if (noiseLocation != null)
        {
            FlipTowardPosition(noiseLocation);
            return true;
        }
        return false;
    }

    private void FlipTowardPosition(Vector3 position)
    {
        if (transform.position.x < position.x)
            FaceRight();
        else
            FaceLeft();
    }

    float currPos = 0f;

    [Task]
    public void MoveTo_Destination()
    {
        if (Task.current.isStarting)
            currPos = 0f;
        //transform.Translate(waypointTarget - transform.position * Time.deltaTime * 2.0f);
        //currPos += 0.1f * Time.deltaTime;
        //transform.position = Vector3.Lerp(transform.position, waypointTarget, currPos);
        Vector3 dir = Vector3.Normalize(waypointTarget - transform.position);
        //transform.Translate(dir * Time.deltaTime);
        rb.MovePosition(transform.position + dir * Time.deltaTime * idleSpeed);
        isRunning = false;
        isWalking = true;
        WaitArrival(waypointTarget);
    }

    [Task]
    public void WaitArrival(Vector3 position)
    {
        var task = Task.current;
        float d = Vector2.Distance(transform.position, position);
        if (!task.isStarting && d <= 0.25f)
        {
            task.Succeed();
        }

        if (Task.isInspected)
            task.debugInfo = string.Format("d-{0:0.00}", d);
    }

    [Task]
    bool NextWaypoint()
    {
        if (waypointPath != null)
        {
            waypointIndex++;
            if (Task.isInspected)
                Task.current.debugInfo = string.Format("i={0}", waypointArrayIndex);
        }
        return true;
    }

    [Task]
    bool PlayerInRange()
    {
        bool inRange = Vector2.Distance(transform.position, player.transform.position) <= 0.25f;
        return inRange;
    }

    [Task]
    bool ChasePlayer()
    {
        if (player != null)
        {
            if (transform.position.x < player.transform.position.x)
                FaceRight();
            else
                FaceLeft();
            Vector3 dir = Vector3.Normalize(player.transform.position - transform.position);
            //transform.Translate(dir * Time.deltaTime);
            rb.MovePosition(transform.position + dir * Time.deltaTime * chaseSpeed);
            isRunning = true;
            isWalking = false;
            return true;
        }
        return false;
    }

    [Task]
    bool AttackPlayer()
    {
        if (player != null)
        {
            return true;
        }
        return false;
    }

    int waypointArrayIndex
    {
        get
        {
            int i = 0;
            if (waypointPath.loop)
            {
                i = waypointIndex % waypointPath.waypoints.Length;
            }
            else
            {
                int n = waypointPath.waypoints.Length;
                i = waypointIndex % (n * 2);

                if (i > n - 1)
                    i = (n - 1) - (i % n);
            }

            return i;
        }
    }
}
