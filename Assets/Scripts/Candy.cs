using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public static void Create(Transform candyPrefab, Vector3 spawnPosition, Vector2 shootDirection) {
        Candy candy = Instantiate(candyPrefab, spawnPosition, Quaternion.identity).GetComponent<Candy>();
    
        candy.Setup(shootDirection);
    }

    private float timeToDisappear;

    private void Setup(Vector2 shootDirection) {
        gameObject.GetComponent<Rigidbody2D>().AddForce(120 * shootDirection);
        Debug.Log(shootDirection);

        timeToDisappear = 1.05f;
    }

    private void Update() {
        timeToDisappear -= Time.deltaTime;
        if (timeToDisappear <= 0f) {
            Destroy(gameObject);
        }
    }
}
