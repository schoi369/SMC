using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum TriggerType { Enter, Exit }

    public int health = 3;

    public EnemyHealthBar healthBar;

    /* Changing vision area / hearing area
     * 
     * Vision can be changed on the enemy's LineOfSight child in the heirarchy.
     * This child can be scaled to easily change distance/width. To change the angle
     * click "Edit Collider" on its Polygon Collider 2D component.
     * 
     * Hearing radius can be changed on the enemy's HearingRadius child in the heirarchy.
     * Changing the radius of the Circle Collider 2D component will change its area.
     */

    public GameObject lineOfSight;
    public GameObject losIndicator; //For demo purposes
    public GameObject hearingRadius;
    public GameObject hearIndicator; //For demo purposes

    public MeshRenderer bodyMesh;

    private bool isDead;

    void Start()
    {
        EnemyChildDelegate losChild = lineOfSight.AddComponent<EnemyChildDelegate>();
        losChild.Parent = this;
        losChild.ChildTag = "LoS";

        EnemyChildDelegate earChild = hearingRadius.AddComponent<EnemyChildDelegate>();
        earChild.Parent = this;
        earChild.ChildTag = "Ear";

        healthBar.MaxHealth = health;
    }

    void Update()
    {
        // just for debugging
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Damage(1);
        }
    }

    public void Damage(int amount)
    {
        healthBar.removeHealth(amount);
        health -= amount;
        if (health <= 0)
        {
            bodyMesh.material.color = Color.red;
            losIndicator.SetActive(false);
            hearIndicator.SetActive(false);
            isDead = true;
        }
    }

    public void OnChildTrigger(string childTag, Collider2D collision, TriggerType triggerType)
    {
        if (isDead)
            return;

        switch (childTag)
        {
            case "LoS": // Line of sight triggered
                if (triggerType == TriggerType.Enter)
                    HandleVisionEnter();
                else
                    HandleVisionExit();
                break;

            case "Ear": // Hearing radius triggered
                if (triggerType == TriggerType.Enter)
                    HandleHearingEnter();
                else
                    HandleHearingExit();
                break;

            default:
                throw new MissingReferenceException("Could not find case for child with tag " + childTag);
        }
    }

    private void HandleVisionEnter()
    {
        losIndicator.GetComponent<SpriteRenderer>().color = Color.cyan;
    }

    private void HandleVisionExit()
    {
        losIndicator.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void HandleHearingEnter()
    {
        hearIndicator.GetComponent<SpriteRenderer>().color = Color.magenta;
    }

    private void HandleHearingExit()
    {
        hearIndicator.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
