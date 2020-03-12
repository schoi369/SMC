using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    // Name of this door
    public string doorName;

    // Name of the door in the next scene that is attached to this one
    public string nextDoorName;
    public string nextSceneName;

    public Transform playerSpawn;

    private static bool isTransition;
    private static string nextDoor;

    private void Start()
    {
        if (isTransition)
        {
            if (doorName.Equals(nextDoor))
            {
                FindObjectOfType<BasicMovement>().transform.position = playerSpawn.position;
                isTransition = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            isTransition = true;
            nextDoor = nextDoorName;
            SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
        }
    }
}
