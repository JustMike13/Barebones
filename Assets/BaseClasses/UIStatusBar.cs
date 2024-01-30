using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatusBar : MonoBehaviour
{
    [SerializeField]
    protected float maxValue;
    protected float currentValue;
    protected GameObject UIBar = null;

    void Start()
    {
        Refill();
    }

    public void Refill(float value = Mathf.Infinity)
    {
        if (value == Mathf.Infinity)
        {
            currentValue = maxValue;
        }
        else
        {
            currentValue += value;
            if (currentValue > maxValue)
            {
                currentValue = maxValue;
            }
        }
        SetBarFill(currentValue / maxValue);
    }

    public void SetBarFill(float percentage)
    {
        if (UIBar != null)
        {
            UIBar.GetComponent<Image>().fillAmount = percentage;
        }
    }

    public bool IsFull()
    {
        return currentValue == maxValue;
    }
}
