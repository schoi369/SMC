using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChildDelegate : MonoBehaviour
{
    private Enemy parent;
    private string childTag;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (parent != null && childTag != null && collision.tag.Equals("Player"))
            parent.OnChildTrigger(childTag, collision, Enemy.TriggerType.Enter);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (parent != null && childTag != null && collision.tag.Equals("Player"))
            parent.OnChildTrigger(childTag, collision, Enemy.TriggerType.Exit);
    }

    public Enemy Parent
    {
        set { parent = value; }
    }

    public string ChildTag
    {
        set { childTag = value; }
    }
}
