using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicMovement : MonoBehaviour
{

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer sr;
    // public GameObject projectile;

    public HealthBar healthBar;
    public int maxHealth = 100;
    public static int currentHealth;
    private static bool healthInitialized = false;

    public AlertLevel alertLevel;

    public float meleeRange = 0.85f;
    public float meleeCooldown = 0.0f;
    public float throwCooldown = 0.0f;

    public bool isSneaking = false;
    private bool isAttacking = false;
    private float lastMeleeTime = 0.0f;
    private float throwTime = 0.0f;

    public bool isMoving = false;

    [SerializeField] private Transform candy;

    public Camera mainCam;
    CameraShake cameraShake;

    public GameObject soundDistractionPrefab;

    public void Awake() {
        sr = GetComponent<SpriteRenderer>();
        mainCam = Camera.main;
    }

    // Input
    [SerializeField] private float speed = 2f;
    [SerializeField] private float slowSpeed = 1f;

    void Start() {
        if (!healthInitialized)
        {
            currentHealth = maxHealth;
            healthInitialized = true;
        }
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
        cameraShake = mainCam.GetComponent<CameraShake>();
    }

    Vector2 lastPosition;

    void Update()
    {

        Vector2 currentPosition = this.transform.position;

        if (rb.velocity.Equals(Vector2.zero)) {
            isMoving = false;
        } else {
            isMoving = true;
        }

        int horizontalMovement = 0;
        int verticalMovement = 0;

        if (isAttacking)
            return;

        if (InputMap.Instance.GetInput(Action.ATTACK) && (Time.time >= lastMeleeTime + meleeCooldown))
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
            rb.velocity = Vector2.zero;
            animator.SetFloat("Speed", 0);
            isSneaking = false;
            return;
        }
        // if (InputMap.Instance.GetInputDown(Action.THROW))
        // {

        //     // Candy Type 1: Initial Implementation
        //     // if (Time.time >= throwTime + throwCooldown)
        //     // {
        //     //     throwTime = Time.time;
        //     //     GameObject p = Instantiate(projectile, transform.position, transform.rotation);
        //     // }

        //     // Candy Type 2: Updated Version of Shooting Candy (with directions)
        //     Vector2 shootDirection = (currentPosition - lastPosition).normalized;
        //     if (shootDirection.normalized.Equals(new Vector2(0, 1)) || shootDirection.normalized.Equals(new Vector2(0, -1))) {
        //         return;
        //     }

        //     if (shootDirection.normalized.Equals(Vector2.zero)) {
        //         if (!sr.flipX) {
        //             shootDirection = new Vector2(1, 0);
        //         } else {
        //             shootDirection = new Vector2(-1, 0);
        //         }
        //     }
        //     Candy.Create(candy, this.transform.position, shootDirection);
            
        // }
        if (InputMap.Instance.GetInput(Action.RIGHT))
        {
            horizontalMovement++;
            sr.flipX = false;
        }
        if (InputMap.Instance.GetInput(Action.LEFT))
        {
            horizontalMovement--;
            sr.flipX = true;
        }
        if (InputMap.Instance.GetInput(Action.UP))
        {
            verticalMovement++;
        }
        if (InputMap.Instance.GetInput(Action.DOWN))
        {
            verticalMovement--;
        }
        Vector2 velocity = new Vector2(horizontalMovement, verticalMovement);
        if (InputMap.Instance.GetInput(Action.SLOW))
        {
            animator.SetBool("CtrlPressed", true);
            rb.velocity = velocity.normalized * slowSpeed;
            animator.SetFloat("Speed", velocity.sqrMagnitude);
            isSneaking = true;
        }
        else
        {
            animator.SetBool("CtrlPressed", false);
            rb.velocity = velocity.normalized * speed;
            animator.SetFloat("Speed", velocity.sqrMagnitude);
            isSneaking = false;
        }
        // if (InputMap.Instance.GetInputDown(Action.STOMP))
        // {
        //     GameObject sdClone = Instantiate(soundDistractionPrefab, transform);
        // }

        lastPosition = currentPosition;

        ScoreManagerScript.Instance.leftHealth = currentHealth;

    }

    public void MeleeAttack()
    {
        int layerMask = ~((1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Indicator")));
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, transform.right * (sr.flipX ? -1 : 1), meleeRange, layerMask);
        
        if (hit && hit.collider.gameObject.tag.Equals("Enemy"))
        {
            Enemy hitEnemy = hit.collider.gameObject.GetComponent<Enemy>();
            
            if (hitEnemy.SpriteRenderer.flipX == sr.flipX)
                hitEnemy.Damage(3);
            else
                hitEnemy.Damage(1);
            
            TakeDamage(10);
        }

        lastMeleeTime = Time.time;
        isAttacking = false;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0)
            SceneManager.LoadScene("BadEnd");
        healthBar.SetHealth(currentHealth);
        cameraShake.Shake(0.05f, 0.2f);
    }
}
