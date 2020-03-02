using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    public List<GameObject> healthBars;
    public SpriteMask healthMask;

    private int maxHealth;
    private int currHealth;
    private bool isAnimating;
    private float animStartTime;
    private float animTimeRemaining;

    // Start is called before the first frame update
    void Start()
    {
        if (healthBars.Count == 0)
            throw new System.Exception("Enemy health bar does not have any bars.");

        isAnimating = false;
    }

    public void removeHealth(int amount)
    {
        if (currHealth >= 0) {
            for (int i = 0; i < amount && currHealth > 0; i++)
            {
                healthBars[currHealth - 1].GetComponent<SpriteRenderer>().color = Color.white;
                currHealth--;
                StopAllCoroutines();
                StartCoroutine(AnimateHealthRemoval(currHealth));
                
            }
        }
    }

    IEnumerator AnimateHealthRemoval(int healthBar)
    {
        float startDelay = 0.25f;
        float animSpeed = 0.05f;

        //if (currHealth > 0) //Enable this to make HP bar instantly begin to drain if at 0 health
        {
            if (!isAnimating)
            {
                animStartTime = Time.time;
                animTimeRemaining = startDelay;
                isAnimating = true;
                yield return new WaitForSeconds(animTimeRemaining);
            }
            else
            {
                if (Time.time - animStartTime < animTimeRemaining)
                {
                    animTimeRemaining = Time.time - animStartTime;
                    animStartTime = Time.time;
                    yield return new WaitForSeconds(animTimeRemaining);
                }
            }
        }

        animTimeRemaining = -1.0f;

        float currProgress = 0f;
        while (currProgress <= 0.65f)
        {
            healthMask.transform.position = Vector2.Lerp(healthMask.transform.position, healthBars[healthBar].transform.position, currProgress);
            currProgress += animSpeed;
            yield return new WaitForSeconds(animSpeed / 2.0f);
        }
        healthMask.transform.position = healthBars[healthBar].transform.position;

        isAnimating = false;

        if (currHealth <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    public int MaxHealth
    {
        set
        {
            currHealth = maxHealth = value;
        }
    }
}
