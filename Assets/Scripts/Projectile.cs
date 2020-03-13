using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;


public class Projectile : MonoBehaviour
{

    [Tooltip("Position we want to hit")]
    public Vector3 targetPos;

    [Tooltip("Horizontal speed, in units/sec")]
    public float speed = 10;
    private float thrust = 10.0f;
    public float duration = 10f;

    Vector3 startPos;

    void Start()
    {
        StartCoroutine(MoveSide());
        Destroy(gameObject, 2.1f);
    }


    IEnumerator MoveSide()
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.Translate(Vector3.right * Time.deltaTime);
            transform.Translate(Vector3.up * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("I");
    }
}