using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    public GameObject healthPoint;
    public GameObject pointAnchor;

    private int maxHealth;
    private int currHealth;

    private List<GameObject> healthPointMarkers;

    // Start is called before the first frame update
    void Start()
    {
        healthPointMarkers = new List<GameObject>();
    }

    public void removeHealth(int amount)
    {
        if (currHealth - amount <= 0)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            for (int i = 0; i < amount; i++)
            {
                healthPointMarkers[currHealth - 1].SetActive(false);
                currHealth--;
            }
        }
    }

    public int MaxHealth
    {
        set {
            maxHealth = value;
            currHealth = maxHealth;

            healthPointMarkers.Clear();

            var currRenderer = GetComponent<SpriteRenderer>();

            float xScale = healthPoint.transform.localScale.x / maxHealth ;

            healthPoint.SetActive(true);
            
            // Fill bar with points
            // TODO - This code is crude right now, once we have health bar art it will look better
            for (int i = 0; i < maxHealth; i++)
            {
                GameObject point = Instantiate(healthPoint, pointAnchor.transform, false);
                point.transform.localScale = new Vector3(xScale / 1.15f, point.transform.localScale.y, point.transform.localScale.z);
                point.transform.position = new Vector3(point.transform.position.x + i*(0.075f/maxHealth) + (i * healthPoint.GetComponent<SpriteRenderer>().bounds.size.x * point.transform.localScale.x),
                    point.transform.position.y, point.transform.position.z);
                healthPointMarkers.Add(point);
            }
            pointAnchor.transform.Translate(Vector3.right * (healthPoint.GetComponent<SpriteRenderer>().bounds.size.x * xScale / 2));
            healthPoint.SetActive(false);
        }
    }
}
