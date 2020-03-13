using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public Transform target;
    public Vector2 minPosition;
    public Vector2 maxPosition;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<BasicMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position != target.position) {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, target.position.z);

            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
            
            transform.position = new Vector3(targetPosition.x, targetPosition.y, -10);
        }
    }
}
