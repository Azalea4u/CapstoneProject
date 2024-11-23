using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ItemManager itemManager;
    public InventoryManager inventoryManager;
    public Player player;

    public bool isGamePaused = false;

    [Header("Starting Game")]
    [SerializeField] private bool firstGame = true;

    [Header("UI")]
    [SerializeField] public TMPro.TextMeshProUGUI LevelText;
    [SerializeField] private Int_SO currentLevel;
    [SerializeField] public TMPro.TextMeshProUGUI GoldText;
    [SerializeField] private Int_SO currentGold;

    private Vector3 crouchingOffset = new Vector3(0, -2, -10); // Adjust as needed for the crouch position

    public int Level
    {
        get { return currentLevel.value; }
        set { currentLevel.value = value; }
    }

    public int Gold
    {
        get { return currentGold.value; }
        set { currentGold.value = value; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }        
        DontDestroyOnLoad(this.gameObject);
        
        itemManager = GetComponent<ItemManager>();
        //timeManager = GetComponent<TimeManager>();

        player = FindAnyObjectByType<Player>();
    }

    public void Start()
    {
        //startMenu.SetActive(true);
        //PauseGame();

        if (firstGame)
        {
            Level = 1; 
            Gold = 200;
        }
    }

    private void Update()
    {
        GoldText.text = "Gold: " + currentGold;

        if (SceneManager.GetActiveScene().name == "Game_Level")
            LevelText.text = "Level " + Level;

        if (SceneManager.GetActiveScene().name == "Rest_Level")
        {
            LevelText.text = "Rest Level";
        }
    }

    public void StartGame()
    {
        // Hide the start menu
        //startMenu.SetActive(false);
        Load_Level("Game_Level");

        // Resume the game
        //ResumeGame();
    }

    public void Enter_Store()
    {
        // Pause the game when entering the store
        PauseGame();
    }

    public void Exit_Store()
    {
        // Resume the game when exiting the store
        ResumeGame();
    }

    public void PauseGame()
    {
        isGamePaused = true;
        PlayerMovement.instance.canMove = false;
        PlayerMovement.instance.rb.velocity = Vector2.zero;
        //EnemyBase.instance.rb.velocity = Vector2.zero;
        //EnemyBase.instance.canMove = false;
        //timeManager.PauseTime();
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        //EnemyBase.instance.canMove = true;
        PlayerMovement.instance.canMove = true;
        //timeManager.ResumeTime();
        Debug.Log("Game Resumed");
    }

    public void Load_RestLevel()
    {
        SceneManager.LoadScene("Rest_Level");
    }

    public void Load_GameLevel()
    {
        firstGame = false;
        SceneManager.LoadScene("Game_Level");

        // update the level number
        LevelText.text = "Level " + Level++;
    }

    public void Load_Level(string levelName)
    {
        SceneManager.LoadScene(levelName);

        if (levelName == "Game_Level")
        {
            LevelText.text = "Level " + Level++;
        }
        else
        {
            LevelText.text = "Rest Level";
        }
    }

    public void Load_TestLevel()
    {
        Load_Level("Test_Level");
    }

    public void Quit_Game()
    {
        Application.Quit();
    }
}
