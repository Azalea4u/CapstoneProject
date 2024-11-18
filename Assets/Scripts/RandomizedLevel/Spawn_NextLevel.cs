using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawn_NextLevel : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(2))
            {
                if (SceneManager.GetActiveScene().name == "Game_Level")
                    GameManager.instance.Load_RestLevel();
                else if (SceneManager.GetActiveScene().name == "Rest_Level")
                    GameManager.instance.Load_NextLevel();
            }
        }
    }
}
