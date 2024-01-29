using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : UIStatusBar
{
    [SerializeField]
    float startingValue;
    [SerializeField]
    float hitBonus;

    void Start()
    {
        UIBar = GameObject.FindGameObjectWithTag("PlayerMana");
        currentValue = 0;
        AddMana(startingValue);
    }

    public void ConsumeMana(float ConsumedValue = 5)
    {
        currentValue -= ConsumedValue;
        SetBarFill(currentValue / maxValue);
        if (currentValue <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void AddMana(float AddedValue)
    {
        currentValue += AddedValue;
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }
        SetBarFill(currentValue / maxValue);
    }

    internal void AddHitBonus()
    {
        AddMana(hitBonus);
    }
}
