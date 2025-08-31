using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    public Slider healthSlider;  // HealthBar ����
    public Slider hungerSlider;  // HungerBar ����
    public Slider thirstSlider;  // ThirstBar ����
    public Slider staminaSlider; // StaminaBar ����

    public PlayerStats playerStats;
    void Start()
    {
        // �޼���� �̺�Ʈ ����
        playerStats.OnHpChanged += UpdateHp;
        playerStats.OnStaminaChanged += UpdateStamina;
        playerStats.OnHungerChanged += UpdateHunger;
        playerStats.OnThirstChanged += UpdateThirst;

        // �ʱⰪ ����
        UpdateHp(playerStats.currentHp / playerStats.maxHp);
        UpdateStamina(playerStats.currentStamina / playerStats.maxStamina);
        UpdateHunger(playerStats.currentHunger / playerStats.maxHunger);
        UpdateThirst(playerStats.currentThirst / playerStats.maxThirst);
    }

    void UpdateHp(float ratio)
    {
        healthSlider.value = ratio;
    }

    void UpdateStamina(float ratio)
    {
        staminaSlider.value = ratio;
    }

    void UpdateHunger(float ratio)
    {
        hungerSlider.value = ratio;
    }

    void UpdateThirst(float ratio)
    {
        thirstSlider.value = ratio;
    }

    private void OnDestroy()
    {
        // �̺�Ʈ ����
        playerStats.OnHpChanged -= UpdateHp;
        playerStats.OnStaminaChanged -= UpdateStamina;
        playerStats.OnHungerChanged -= UpdateHunger;
        playerStats.OnThirstChanged -= UpdateThirst;
    }
}
