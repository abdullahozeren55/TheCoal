using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
    public event Action OnHealthZero;
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxPoise;
    private float currentHealth;
    private float currentPoise;

    public float lastPoiseDamageTime { get; private set; }
    public float lastPoiseDamageRegTime { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        currentHealth = maxHealth;
        currentPoise = maxPoise;
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;

        if(currentHealth <= 0)
        {
            currentHealth = 0;

            OnHealthZero?.Invoke();

            Debug.Log("Health is zero!");
        }
    }

    public void IncreaseHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
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
