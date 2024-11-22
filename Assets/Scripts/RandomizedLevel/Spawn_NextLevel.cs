using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawn_NextLevel : MonoBehaviour
{
    [SerializeField] private string next_Scene;

    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    private void Start()
    {
        visualCue.SetActive(false);   
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            visualCue.SetActive(true);

            if (Input.GetKey(KeyCode.Q) || Input.GetMouseButton(2))
            {
                GameManager.instance.Load_Level(next_Scene);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            visualCue.SetActive(false);
        }
    }
}
