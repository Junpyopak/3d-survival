using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    public Slider healthSlider;  // HealthBar 연결
    public Slider hungerSlider;  // HungerBar 연결
    public Slider thirstSlider;  // ThirstBar 연결
    public Slider staminaSlider; // StaminaBar 연결

    public PlayerStats playerStats;
    void Start()
    {
        // 메서드로 이벤트 구독
        playerStats.OnHpChanged += UpdateHp;
        playerStats.OnStaminaChanged += UpdateStamina;
        playerStats.OnHungerChanged += UpdateHunger;
        playerStats.OnThirstChanged += UpdateThirst;

        // 초기값 세팅
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
        // 이벤트 해제
        playerStats.OnHpChanged -= UpdateHp;
        playerStats.OnStaminaChanged -= UpdateStamina;
        playerStats.OnHungerChanged -= UpdateHunger;
        playerStats.OnThirstChanged -= UpdateThirst;
    }
}
