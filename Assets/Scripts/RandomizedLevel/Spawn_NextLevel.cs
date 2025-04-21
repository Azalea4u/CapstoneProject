using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

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
                //GameManager.instance.Load_Level(next_Scene);

                if (next_Scene == "Game_Level") // WHEN LEAVING REST LEVEL
                {
                    GameManager.instance.playerUI.Level++;
                    if (GameManager.instance.shopManager.DoubleGold)
                    {
                        GameManager.instance.playerUI.IsDoubleGold = true;
                    }
                    GameManager.instance.Load_GameLevel();
                    //GameManager.instance.GameLevel_ON();
                }
                else // WHEN LEAVING GAME LEVEL
                {
                    //GameManager.instance.playerUI.LevelText.text = "Rest Level";
                    GameManager.instance.RestingLevel_ON();
                    GameManager.instance.playerUI.IsDoubleGold = false;
                }
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
