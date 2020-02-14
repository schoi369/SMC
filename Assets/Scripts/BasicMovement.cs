using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public bool faceRight = true;
    public bool isMoving = false;

    public SpriteRenderer sr;

    public void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    [SerializeField]
    private float speed = 3;
    [SerializeField]
    private float slowSpeed = 1;
    private Vector3 velocity;
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
        velocity = new Vector3(horizontalMovement, verticalMovement, 0);
        if (InputMap.Instance.GetInput(Action.SLOW))
        {
            transform.position += velocity.normalized * Time.deltaTime * slowSpeed;
            animator.SetFloat("Speed", velocity.sqrMagnitude);
        }
        else
        {
            transform.position += velocity.normalized * Time.deltaTime * speed;
            animator.SetFloat("Speed", velocity.sqrMagnitude);
        }
    }
}
