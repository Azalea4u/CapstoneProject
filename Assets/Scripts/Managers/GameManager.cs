using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ItemManager itemManager;
    public TimeManager timeManager;
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
        timeManager = GetComponent<TimeManager>();

        player = FindAnyObjectByType<Player>();
    }

    public void Start()
    {
        //startMenu.SetActive(true);
        //PauseGame();
        levelText.text = "Level " + Level;

        Debug.Log("Level" + Level);
    }

    private void Update()
    {
        if (!isGamePaused)
        {
            //
        }

        goldText.text = "Gold: " + Gold;
    }

    public void StartGame()
    {
        // Hide the start menu
        startMenu.SetActive(false);

        // Resume the game
        ResumeGame();
        timeManager.StartTimer();
    }

    public void EnterInventory()
    {
        // Pause the game when entering the store
        PauseGame();
    }

    public void ExitInventory()
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

    public void LoadNextLevel()
    {
        // reload the scene
        //UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // update the level number
        levelText.text = "Level " + Level++;
    }
}
