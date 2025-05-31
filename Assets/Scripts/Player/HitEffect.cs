using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HitEffect : MonoBehaviour
{
    [SerializeField] private float duration = 0.25f;

    private int hitEffectAmount = Shader.PropertyToID("_HitEffectAmount");

    private Player player;
    private SpriteRenderer[] spriteRenderers;
    private Material[] materials;
    private float lerpAmount;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        materials = new Material[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            materials[i] = spriteRenderers[i].material;
        }
    }
    public void TriggerHitEffect()
    {
        lerpAmount = 0f; // Reset the lerp amount

        DOTween.To(GetLerpValue, SetLerpValue, 1f, duration)
            .SetEase(Ease.OutExpo)
            .OnUpdate(OnLerpUpdate)
            .OnComplete(OnLerpComplete); // Corrected this line
    }

    private void OnLerpUpdate()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat(hitEffectAmount, GetLerpValue());
        }
    }

    private void OnLerpComplete()
    {
        DOTween.To(GetLerpValue, SetLerpValue, 0f, duration).OnUpdate(OnLerpUpdate);
    }

    private float GetLerpValue()
    {
        return lerpAmount;
    }

    private void SetLerpValue(float newValue)
    {
        lerpAmount = newValue;
    }
}
