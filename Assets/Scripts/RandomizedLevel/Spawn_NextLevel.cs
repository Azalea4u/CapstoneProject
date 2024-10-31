using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_NextLevel : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player has reached the end of the level");

            if (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(2))
            {
                Debug.Log("Player has reached the end of the level");
                GameManager.instance.LoadNextLevel();
            }
        }
    }
}
