using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Status")]
    public float maxHp = 100f;
    public float currentHp;

    public float maxStamina = 100f;
    public float currentStamina;

    public float maxHunger = 100f;
    public float currentHunger;

    public float maxThirst = 100f;
    public float currentThirst;

    [Header("Drain/Recovery Rates")]
    public float hungerDrainRate = 0.0167f; // 1분에 1 소모
    public float thirstDrainRate = 0.025f;  // 1분에 1.5 소모
    public float staminaRecoveryRate = 15f; // 앉으면 15/s 회복

    public event Action<float> OnHpChanged;
    public event Action<float> OnStaminaChanged;
    public event Action<float> OnHungerChanged;
    public event Action<float> OnThirstChanged;

    [HideInInspector] public PlayerMovement playerMovement;

    void Start()
    {
        currentHp = maxHp;
        currentStamina = maxStamina;
        currentHunger = maxHunger;
        currentThirst = maxThirst;

        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // 배고픔/수분 감소
        currentHunger -= hungerDrainRate * Time.deltaTime;
        currentThirst -= thirstDrainRate * Time.deltaTime;

        // 배고픔/수분이 0이면 체력 감소
        if (currentHunger <= 0 || currentThirst <= 0)
            TakeDamage(5f * Time.deltaTime);

        // 앉아있으면 스테미나 회복
        if (playerMovement != null && playerMovement.isSitting && currentStamina < maxStamina)
        {
            RecoverStamina(staminaRecoveryRate * Time.deltaTime);
        }

        // Clamp
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        currentThirst = Mathf.Clamp(currentThirst, 0, maxThirst);

        // 이벤트 호출
        OnHpChanged?.Invoke(currentHp / maxHp);
        OnStaminaChanged?.Invoke(currentStamina / maxStamina);
        OnHungerChanged?.Invoke(currentHunger / maxHunger);
        OnThirstChanged?.Invoke(currentThirst / maxThirst);
    }

    public void TakeDamage(float amount)
    {
        currentHp -= amount;
    }

    public void UseStamina(float amount)
    {
        currentStamina -= amount;
    }

    public void RecoverStamina(float amount)
    {
        currentStamina += amount;
        if (currentStamina > maxStamina)
            currentStamina = maxStamina;
    }

    public void Eat(float amount)
    {
        currentHunger += amount;
    }

    public void Drink(float amount)
    {
        currentThirst += amount;
    }

    public void RestoreHp(float amount)
    {
        currentHp += amount;
    }
}
