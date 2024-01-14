using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : CoreComponent
{
    public event Action OnHealthZero;
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxPoise;
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private Slider easeHealthBarSlider;
    private float currentHealth;
    private float currentPoise;

    public float lastPoiseDamageTime { get; private set; }
    public float lastPoiseDamageRegTime { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        currentHealth = maxHealth;
        currentPoise = maxPoise;
        if(healthBarSlider != null && easeHealthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            easeHealthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
            easeHealthBarSlider.value = currentHealth;
        }
    }

    private void Update()
    {
        if(easeHealthBarSlider != null && healthBarSlider != null)
        {
            if(easeHealthBarSlider.value != currentHealth)
            {
                easeHealthBarSlider.value = Mathf.Lerp(easeHealthBarSlider.value, currentHealth, 0.05f);

                if(easeHealthBarSlider.value - currentHealth <= maxHealth/200f)
                {
                    easeHealthBarSlider.value = currentHealth;
                }
            }

            
        }
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;
        if(healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
        }

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            if(easeHealthBarSlider != null)
            {
                easeHealthBarSlider.value = currentHealth;
            }

            OnHealthZero?.Invoke();

            Debug.Log("Health is zero!");
        }
    }

    public void IncreaseHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        if(healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
        }
    }

    public void DecreasePoise(float amount)
    {
        currentPoise -= amount;
        lastPoiseDamageTime = Time.time;
    }

    public void IncreasePoise(float amount)
    {
        currentPoise = Mathf.Clamp(currentPoise + amount, -(Mathf.Infinity), maxPoise);
        lastPoiseDamageRegTime = Time.time;
    }

    public bool Stun()
    {
        return currentPoise <= 0;
    }

    public bool ShouldRegenPoise()
    {
        return currentPoise < maxPoise;
    }
}
