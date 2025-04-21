using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHunger : MonoBehaviour
{
    [SerializeField] private HungerData hungerData;
    [SerializeField] private float slowHungerDuration = 2f; // Duration to slow hunger after eating

    private float currentTickInterval;
    private float hungerTimer;

    private void Start()
    {
        // Use the HungerSpeed from HungerData
        currentTickInterval = hungerData.HungerSpeed;
        hungerTimer = 0f;
    }

    private void Update()
    {
        if (hungerData.Hunger > 0 && (!GameManager.instance.isGamePaused || !DialogueManager.instance.dialogueIsPlaying))
        {
            hungerTimer += Time.deltaTime;

            if (hungerTimer >= currentTickInterval)
            {
                ReduceHunger();
                hungerTimer = 0f;
            }
        }
        else if (hungerData.Hunger <= 20)
        {
            Debug.Log("Player is starving!");
            // Handle starvation effects here, e.g., damage or reduced movement.
        }
    }

    private void ReduceHunger()
    {
        hungerData.Hunger = Mathf.Max(0, hungerData.Hunger - 1);
        //Debug.Log($"Hunger: {hungerData.Hunger}");
    }

    public void Eat(float foodValue)
    {
        hungerData.Hunger = Mathf.Min(100, hungerData.Hunger + foodValue);
        Debug.Log($"Player ate! Hunger: {hungerData.Hunger}");
        StartCoroutine(SlowHungerRate());
    }

    private IEnumerator SlowHungerRate()
    {
        float originalInterval = currentTickInterval;
        currentTickInterval = hungerData.HungerSpeed * 2; // Slow down hunger rate temporarily

        yield return new WaitForSeconds(slowHungerDuration);

        currentTickInterval = hungerData.HungerSpeed; // Restore original rate
    }

    public float GetHungerPercentage()
    {
        return hungerData.Hunger / 100f; // Normalize hunger for UI
    }
}
