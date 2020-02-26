using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{

    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;
    private float moveSpeed = 2f;
    private float walkSpeed = 2f;
    private float sneakSpeed = 0.75f;

    public bool faceRight = true;
    public bool isMoving = false;


    public SpriteRenderer sr;

    public void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    // Input
    [SerializeField]
    private float speed = 3;
    [SerializeField]
    private float slowSpeed = 1;
    // Start is called before the first frame update
    void Update()
    {
        int horizontalMovement = 0;
        int verticalMovement = 0;
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
        }
        else
        {
            animator.SetBool("CtrlPressed", false);
            rb.velocity = velocity.normalized * speed;
            animator.SetFloat("Speed", velocity.sqrMagnitude);
        }
    }
}
