using System;
using UnityEngine;

public class Entity_Stamina : MonoBehaviour
{
    public event Action<float, float> OnStaminaChanged;

    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float currentStamina;
    [SerializeField] private float blockRegenMultiplier = 0.3f;

    [Header("Regen Settings")]
    [SerializeField] private float regenRate = 15f;
    [SerializeField] private float regenDelay = 1f;

    private float regenTimer;

    private void Awake()
    {
        currentStamina = maxStamina;
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
    }

    private void Update()
    {
        HandleRegen();
    }

    private void HandleRegen()
    {
        if (currentStamina >= maxStamina)
            return;

        regenTimer += Time.deltaTime;

        if (regenTimer < regenDelay)
            return;

        float regen = regenRate;

        Player player = GetComponent<Player>();
        if (player != null && player.stateMachine.currentState == player.blockState)
        {
            regen *= blockRegenMultiplier;
        }

        currentStamina += regen * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
    }

    public bool CanUseStamina(float cost)
    {
        return currentStamina >= cost;
    }

    public void UseStamina(float cost)
    {
        currentStamina -= cost;

        if (currentStamina < 0)
            currentStamina = 0;

        regenTimer = 0f;

        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
    }

    public bool HasStamina()
    {
        return currentStamina > 0;
    }

    public float GetCurrentStamina() => currentStamina;
    public float GetMaxStamina() => maxStamina;
}