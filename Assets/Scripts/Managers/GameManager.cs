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
    [SerializeField] private Bool_SO FirstGame;

    [Header("UI")]
    [SerializeField] public GameObject GameOver_Panel;
    [SerializeField] public GameObject PausedMenu_Panel;
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
        player = FindAnyObjectByType<Player>();
    }

    public void Start()
    {
        if (firstGame)
        {
            //Level = 1; 
            //Gold = 200;
        }
        GameOver_Panel.SetActive(false);
        PausedMenu_Panel.SetActive(false);
    }

    private void Update()
    {
        GoldText.text = currentGold.value.ToString();
        firstGame = FirstGame.Value;

        if (SceneManager.GetActiveScene().name == "Game_Level")
        {
            LevelText.text = "Level " + Level;
        }

        if (Input.GetKeyDown(KeyCode.Tab) && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            isGamePaused = !isGamePaused;

            if (isGamePaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (SceneManager.GetActiveScene().name == "Rest_Level")
        {
            LevelText.text = "Rest Level";
        }
    }

    public void StartMenu()
    {
        firstGame = true;
        Load_Level("Start_Menu");
    }

    public void StartGame()
    {
        Load_Level("Game_Level");
    }

    public void Enter_Store()
    {
        PauseGame();
    }

    public void Exit_Store()
    {
        ResumeGame();
    }

    public void PauseGame()
    {
        PausedMenu_Panel.SetActive(true);
        isGamePaused = true;

        PlayerMovement.instance.StopMovement();
        PlayerMovement.instance.rb.velocity = Vector2.zero;
        if (SceneManager.GetActiveScene().name == "Game_Level")
        {
            EnemyBase.instance.rb.velocity = Vector2.zero;
            EnemyBase.instance.canMove = false;
        }
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        PausedMenu_Panel.SetActive(false);
        isGamePaused = false;

        PlayerMovement.instance.ContinueMovement();
        if (SceneManager.GetActiveScene().name == "Game_Level")
        {
            EnemyBase.instance.canMove = true;
        }
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
