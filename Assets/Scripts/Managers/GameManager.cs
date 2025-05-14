using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Managers")]
    public ShopManager shopManager;
    public ItemManager itemManager;
    public DialogueManager dialogueManager;
    public InventoryManager inventoryManager;

    public Player player;
    public GameObject playerPrefab;
    public bool isGamePaused = false;

    [Header("Starting Game")]
    [SerializeField] public Bool_SO FirstGame;

    [Header("UI")]
    [SerializeField] private GameObject controlPanel;
    [SerializeField] public Player_UI playerUI;
    [SerializeField] public GameObject playerUI_prefab;

    [Header("Levels")]
    [SerializeField] public GameObject Start_Menu;
    [SerializeField] public GameObject Game_Level;
    [SerializeField] public GameObject Resting_Level;
    private GameObject currentGameLevel;

    private Vector3 crouchingOffset = new Vector3(0, -2, -10); // Adjust as needed for the crouch position

    private void Awake()
    {
        // Singleton pattern for GameManager
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        //DontDestroyOnLoad(gameObject);

        FirstGame.Value = true;
        itemManager = GetComponent<ItemManager>();

        player = FindAnyObjectByType<Player>();
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape)) && !controlPanel.activeSelf
            && !DialogueManager.instance.dialogueIsPlaying && SceneManager.GetActiveScene().name != "Start_Menu")
        {
            isGamePaused = !isGamePaused;

            if (isGamePaused)
            {
                playerUI.PausedMenu_Panel.SetActive(true);
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("Game_Level");
    }

    public void ControlPanel()
    {
        controlPanel.SetActive(!controlPanel.activeSelf);
    }

    public void Enter_Store()
    {
        PauseGame();
    }

    public void Exit_Store()
    {
        ResumeGame();
    }

    public void Load_StartMenu()
    {
        inventoryManager.ClearHotBarData();
        FirstGame.Value = true;
        Load_Level("Start_Menu");
    }

    #region PAUSE/RESUME
    public void PauseGame()
    {
        isGamePaused = true;

        PlayerMovement.instance.StopMovement();
        //PlayerMovement.instance.rb.velocity = Vector2.zero;
        if (SceneManager.GetActiveScene().name == "Game_Level")
        {
            if (Game_Level.activeSelf == true)
            {
                EnemyBase.instance.rb.velocity = Vector2.zero;
                EnemyBase.instance.canMove = false;
            }
        }
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        playerUI.PausedMenu_Panel.SetActive(false);
        isGamePaused = false;

        PlayerMovement.instance.ContinueMovement();
        if (SceneManager.GetActiveScene().name == "Game_Level")
        {
            EnemyBase.instance.canMove = true;
        }
        Debug.Log("Game Resumed");
    }
    #endregion

    #region SCENES
    public void Load_RestLevel()
    {
        SceneManager.LoadScene("Rest_Level");
    }

    public void Load_GameLevel()
    {

        FirstGame.Value = false;
        SceneManager.LoadScene("Game_Level");
        if (SceneManager.GetActiveScene().name == "Game_Level")
        {
            inventoryManager.RefreshHotBarData();
        }
        inventoryManager.Add("Hotbar", GameManager.instance.itemManager.GetItemByName("Bomb"));
        //playerUI_prefab.GetComponent<Hotbar_UI>().LoadHotBarFromData();
    }

    public void Load_Level(string levelName)
    {
        SceneManager.LoadScene(levelName);
        if (FirstGame.Value)
        {
            inventoryManager.Add("Hotbar", GameManager.instance.itemManager.GetItemByName("Bomb"));
            playerUI.IsDoubleGold = false; 
        }

        FirstGame.Value = false;
    }

    private void Load_TestLevel()
    {
        Load_Level("Test_Level");
    }
    #endregion

    #region CHANGE_LEVEL
    public void GameLevel_ON()
    {
        if (SceneManager.GetActiveScene().name == "Game_Level")
        {
            inventoryManager.RefreshHotBarData();
            playerUI_prefab.GetComponent<Hotbar_UI>().LoadHotBarFromData();
        }

        // Instantiate the Game_Level prefab if it doesn't already exist
        if (currentGameLevel == null && Game_Level != null)
        {
            currentGameLevel = Instantiate(Game_Level);
        }

        playerUI_prefab.SetActive(true);
        currentGameLevel.SetActive(true); // Activate the new Game_Level
        Resting_Level.SetActive(false);
        Start_Menu.SetActive(false);

        playerPrefab.SetActive(true);
        Debug.Log("Level:" + playerUI.Level);
        playerUI.Level++;
        Debug.Log("Level:" + playerUI.Level);
    }


    public void RestingLevel_ON()
    {
        if (currentGameLevel != null)
        {
            Destroy(currentGameLevel); // Destroy the current Game_Level
            currentGameLevel = null;
        }


        Resting_Level.SetActive(true);
        Game_Level.SetActive(false);
        Start_Menu.SetActive(false);

        playerPrefab.SetActive(true);
        Transform playerSpawn = Resting_Level.transform.Find("PlayerSpawn_RestLevel");
        player.transform.position = playerSpawn.position;
    }
    #endregion

    #region UPDATE_UI

    public void Update_GoldText()
    {
        playerUI.GoldText.text = playerUI.Gold.ToString();
    }

    public void Update_LevelText()
    {
        if (SceneManager.GetActiveScene().name == "Game_Level")
        {
            playerUI.LevelText.text = "Level " + playerUI.Level;
        }
        if (SceneManager.GetActiveScene().name == "Rest_Level")
        {
            playerUI.LevelText.text = "Rest Level";
        }
    }

    #endregion

    public void Quit_Game()
    {
        Application.Quit();
    }
}
