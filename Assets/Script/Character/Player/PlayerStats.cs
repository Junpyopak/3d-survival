using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Status")]
    public float maxHp = 100f;
    public float currentHp;

    public float maxStamina = 120f;
    public float currentStamina;

    public float maxHunger = 150f;
    public float currentHunger;

    public float maxThirst = 150f;
    public float currentThirst;

    public float staminaRecoveryRate = 1f; // 앉으면 회복

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
       
        // 배고픔/수분이 0이면 체력 감소
        if (currentHunger <= 0 || currentThirst <= 0)
            TakeDamage(5f * Time.deltaTime);

        //// 앉아있으면 스테미나 회복
        //if (playerMovement != null && playerMovement.isSitting && currentStamina < maxStamina)
        //{
        //    RecoverStamina(staminaRecoveryRate * Time.deltaTime);
        //}

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

    public void UseHunger(float amount)
    {
        currentHunger -= amount;
    }

    public void UseThirst(float amount)
    {
        currentThirst -= amount;
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
