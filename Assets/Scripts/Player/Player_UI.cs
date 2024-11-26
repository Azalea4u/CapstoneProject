using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_UI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] public GameObject GameOver_Panel;
    [SerializeField] public GameObject PausedMenu_Panel;

    [Header("UI")]
    [SerializeField] public TMPro.TextMeshProUGUI LevelText;
    [SerializeField] private Int_SO currentLevel;
    [SerializeField] public TMPro.TextMeshProUGUI GoldText;
    [SerializeField] private Int_SO currentGold;
    [SerializeField] private Int_SO currentHealth;

    public static Player_UI instance;

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

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Start_Menu")
        {
            Level = 1;
            Gold = 200;
            currentHealth.value = 3;
        }

        GameOver_Panel.SetActive(false);
        PausedMenu_Panel.SetActive(false);
    }

    private void Update()
    {
        GoldText.text = currentGold.value.ToString();
        LevelText.text = "Level " + Level;

        if (SceneManager.GetActiveScene().name == "Rest_Level")
        {
            LevelText.text = "Rest Level";
        }
    }
}
