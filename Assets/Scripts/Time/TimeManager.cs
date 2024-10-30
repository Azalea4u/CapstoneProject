using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("Internal Clock")]
    [SerializeField] public GameTimestamp timestamp;
    [SerializeField] public float timeScale = 1.0f;

    [Header("TimerBar")]
    [SerializeField] private TimerController timerController;

    private bool isTimePaused = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // timestamp = new GameTimestamp(1, 1, 1, 0);
        //StartCoroutine(TimeUpdate());
        PauseTime();
    }

    public void StartTimer()
    {
        StartCoroutine(TimeUpdate());
    }

    private IEnumerator TimeUpdate()
    {
        while (true)
        {
            if (!isTimePaused)
            {
                yield return new WaitForSeconds(1 / timeScale);
                Tick();
            }
            else
            {
                yield return null;
            }
        }
    }

    private void Tick()
    {
        timestamp.UpdateClock();
        UpdateTimerBar();
    }

    private void UpdateTimerBar()
    {
        float fillAmount = 1f - (timestamp.hour - 1 + (float)DateTime.Now.Second / 60f) / 24f;
        timerController.UpdateTimerBar(fillAmount);
    }

    public void PauseTime()
    {
        isTimePaused = true;
    }

    public void ResumeTime()
    {
        isTimePaused = false;
    }

}
