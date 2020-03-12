using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicMovement : MonoBehaviour
{

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer sr;

    public HealthBar healthBar;
    public int maxHealth = 100;
    public static int currentHealth;
    private static bool healthInitialized = false;

    public AlertLevel alertLevel;

    Vector2 movement;

    public float meleeRange = 0.65f;
    public float meleeCooldown = 0.0f;

    public bool faceRight = true;
    public bool isMoving = false;
    public bool isSneaking = false;
    private bool isAttacking = false;
    private float lastMeleeTime = 0.0f;


    public void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    // Input
    [SerializeField]
    private float speed = 3;
    [SerializeField]
    private float slowSpeed = 1;

    void Start() {
        if (!healthInitialized)
        {
            currentHealth = maxHealth;
            healthInitialized = true;
        }
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    void Update()
    {

        // TEST FOR PLAYER HEALTH
        // if(Input.GetKeyDown(KeyCode.LeftAlt)) {
        //     TakeDamage(10);
        // }

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
        }

        lastMeleeTime = Time.time;
        isAttacking = false;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0)
            SceneManager.LoadScene("GameFAIL");
        healthBar.SetHealth(currentHealth);
    }

}
