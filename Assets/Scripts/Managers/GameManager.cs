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

    [Header("GameMenu")]
    public GameObject startMenu;

    public bool isGamePaused = false;

    [SerializeField] private TMPro.TextMeshProUGUI levelText;
    //[SerializeField] private int level = 1;
    [SerializeField] private Int_SO currentLevel;
    [SerializeField] private TMPro.TextMeshProUGUI goldText;
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

        //Level = 1; 
        //Gold = 200;

        Debug.Log("Level" + Level);
    }

    private void Update()
    {
        goldText.text = "Gold: " + Gold;
        levelText.text = "Level " + Level;
        Debug.Log("Level " + Level);
    }

    public void StartGame()
    {
        // Hide the start menu
        //startMenu.SetActive(false);

        // Resume the game
        ResumeGame();
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

    public void Load_NextLevel()
    {
        SceneManager.LoadScene("Game_Level");

        // update the level number
        levelText.text = "Level " + Level++;
    }

    public void Load_Level(string levelName)
    {
        SceneManager.LoadScene(levelName);

        if (levelName == "Game_Level")
        {
            levelText.text = "Level " + Level++;
        }
        else
        {
            levelText.text = "Rest Level";
        }
    }
}