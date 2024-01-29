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

    public void Refill()
    {
        currentValue = maxValue;
        SetBarFill(1f);
    }

    public void SetBarFill(float percentage)
    {
        if (UIBar != null)
        {
            UIBar.GetComponent<Image>().fillAmount = percentage;
        }
    }
}
