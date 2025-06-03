using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawn_NextLevel : MonoBehaviour
{
    [SerializeField] private string next_Scene;

    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    private bool playerInRange = false;

    private void Start()
    {
        visualCue.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Q))
        {
            if (next_Scene == "Game_Level") // WHEN LEAVING REST LEVEL
            {
                GameManager.instance.playerUI.Level++;
                if (GameManager.instance.shopManager.DoubleGold)
                {
                    GameManager.instance.playerUI.IsDoubleGold = true;
                }
                GameManager.instance.Load_GameLevel();
            }
            else // WHEN LEAVING GAME LEVEL
            {
                GameManager.instance.RestingLevel_ON();
                GameManager.instance.playerUI.IsDoubleGold = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            visualCue.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            visualCue.SetActive(false);
        }
    }
}
