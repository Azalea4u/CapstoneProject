using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerBar_UI : MonoBehaviour
{
    [SerializeField] private PlayerHunger playerHunger; // Reference to PlayerHunger script
    [SerializeField] private Slider hungerBar; // Reference to UI Slider

    private void Update()
    {
        if (hungerBar != null && playerHunger != null &&
            (!DialogueManager.instance.dialogueIsPlaying || !GameManager.instance.isGamePaused))
        {
            hungerBar.value = playerHunger.GetHungerPercentage(); // Set slider currentGold
        }
    }
}
