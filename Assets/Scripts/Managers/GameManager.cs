using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ItemManager itemManager;
    public TimeManager timeManager;
    public InventoryManager inventoryManager;

    public Player player;

    [Header("GameMenu")]
    public GameObject startMenu;

    public bool isGamePaused = true;

    [SerializeField] private TMPro.TextMeshProUGUI levelText;
    [SerializeField] private int level = 1;

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
        timeManager = GetComponent<TimeManager>();

        player = FindAnyObjectByType<Player>();
    }

    public void Start()
    {
        startMenu.SetActive(true);
        PauseGame();
    }

    private void Update()
    {
        if (!isGamePaused)
        {
            //
        }
    }

    public void StartGame()
    {
        // Hide the start menu
        startMenu.SetActive(false);

        // Resume the game
        ResumeGame();
        timeManager.StartTimer();
    }

    public void EnterStore()
    {
        // Pause the game when entering the store
        PauseGame();
    }

    public void ExitStore()
    {
        // Resume the game when exiting the store
        ResumeGame();

    }

    public void PauseGame()
    {
        isGamePaused = true;
        PlayerMovement.instance.rb.velocity = Vector2.zero;
        //timeManager.PauseTime();
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        //timeManager.ResumeTime();
        Debug.Log("Game Resumed");
    }

    public void LoadNextlevel()
    {
        // reload the scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        // update the level number
        levelText.text = "Level " + level++;
    }
}
