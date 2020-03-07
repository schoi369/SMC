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

    public float attackCooldown = 3.0f;

    public GameObject lineOfSight;
    public GameObject losIndicator; //For demo purposes
    public GameObject hearingRadius;
    public GameObject hearIndicator; //For demo purposes
    public GameObject attackZone;

    public MeshRenderer bodyMesh;

    private BasicMovement playerTarget;
    private bool isDead;
    private float nextAttackTime;
    private PolygonCollider2D losCollider;
    private SpriteRenderer sr;

    void Start()
    {
        playerTarget = null;
        nextAttackTime = 0.0f;

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

        if (!sr.flipX)
        {
            lineOfSight.transform.Rotate(0f, 0f, 180f);
            attackZone.transform.Rotate(0f, 0f, 180f);
        }
    }

    void Update()
    {
        if (playerTarget != null && Time.time >= nextAttackTime)
        {
            playerTarget.TakeDamage(10);
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    public void Damage(int amount)
    {
        healthBar.removeHealth(amount);
        health -= amount;
        if (health <= 0)
        {
            bodyMesh.material.color = Color.red;
            losIndicator.SetActive(false);
            hearIndicator.SetActive(false);
            this.enabled = false;
            isDead = true;
        }
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
                    HandleVisionExit();
                break;

            case "Ear": // Hearing radius triggered
                if (triggerType == TriggerType.Enter)
                    HandleHearingEnter(collision.gameObject);
                else if (triggerType == TriggerType.Stay)
                    HandleHearingStay(collision.gameObject);
                else
                    HandleHearingExit();
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
            losIndicator.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        else
        {
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

    private void HandleVisionExit()
    {
        losIndicator.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void HandleHearingEnter(GameObject player)
    {
        if (!player.GetComponent<BasicMovement>().isSneaking)
            hearIndicator.GetComponent<SpriteRenderer>().color = Color.magenta;
    }

    private void HandleHearingStay(GameObject player)
    {
        if (!player.GetComponent<BasicMovement>().isSneaking)
            hearIndicator.GetComponent<SpriteRenderer>().color = Color.magenta;
        else
            hearIndicator.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void HandleHearingExit()
    {
        hearIndicator.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public SpriteRenderer SpriteRenderer
    {
        get { return sr; }
    }
}
