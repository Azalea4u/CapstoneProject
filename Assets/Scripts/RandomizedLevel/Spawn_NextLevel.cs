using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawn_NextLevel : MonoBehaviour
{
    [SerializeField] private string next_Scene;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player entered exit");

            if (Input.GetKey(KeyCode.Q) || Input.GetMouseButton(2))
            {
                /*
                if (SceneManager.GetActiveScene().name == "Game_Level")
                    GameManager.instance.Load_RestLevel();
                else if (SceneManager.GetActiveScene().name == "Rest_Level")
                    GameManager.instance.Load_NextLevel();
                */
                GameManager.instance.Load_Level(next_Scene);
            }
        }
    }
}
