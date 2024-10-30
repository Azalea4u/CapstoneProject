using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_NextLevel : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Space))
                GameManager.instance.LoadNextLevel();
        }
    }
}
