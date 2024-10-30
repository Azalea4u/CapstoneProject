using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    [SerializeField] public Slider timerSlider;
   
    public void UpdateTimerBar(float fillAmount)
    {
        timerSlider.value = fillAmount;
    }

}
