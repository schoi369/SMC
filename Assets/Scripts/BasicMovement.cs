using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    Vector2 movement;
    public bool faceRight = true;
    public bool isMoving = false;

    public SpriteRenderer sr;

    public void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    // Input
    void Update() {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        // animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.x < 0) {
            sr.flipX = true;
        } else if (movement.x > 0) {
            sr.flipX = false;
        }
    }

    // Movement
    void FixedUpdate() {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
