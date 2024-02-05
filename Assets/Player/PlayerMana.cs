using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : UIStatusBar
{
    [SerializeField]
    float startingValue;
    [SerializeField]
    float hitBonus;
    [SerializeField]
    float minMana;
    [SerializeField]
    Color StandardManaColor;
    [SerializeField]
    Color InsuficientManaColor;

    void Start()
    {
        UIBar = GameObject.FindGameObjectWithTag("PlayerMana");
        currentValue = 0;
        AddMana(startingValue);
    }

    public void ConsumeMana(float ConsumedValue = 5)
    {
        currentValue -= ConsumedValue;
        if (currentValue <= 0)
        {
            currentValue = 0;
        }
        SetBarFill(currentValue / maxValue);
        UpdateBarColor();
    }

    public void AddMana(float AddedValue)
    {
        currentValue += AddedValue;
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }
        SetBarFill(currentValue / maxValue);
        UpdateBarColor();
    }

    internal void AddHitBonus()
    {
        AddMana(hitBonus);
    }

    public bool IsAvailable(float value)
    {
        return value <= currentValue;
    }

    private void UpdateBarColor()
    {
        UIBar.GetComponent<Image>().color = currentValue < minMana ? InsuficientManaColor : StandardManaColor;
    }
}
