using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDistraction : MonoBehaviour
{
    private void Start()
    {
        Destroy(GetComponent<CircleCollider2D>(), 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy;
        if (collision.tag.Equals("Enemy"))
        {
            enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.HeardSuspiciousNoise(transform.position);
        }
    }
}
