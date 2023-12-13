using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField]
    float maxHealth;
    float currentHealth;
    protected GameObject healthBar = null;

    void Start()
    {
        RefillHealth();
    }

    public void RefillHealth()
    {
        currentHealth = maxHealth;
        SetHealthBar(1f);
    }

    public void TakeDamage(int damageTaken = 5)
    {
        currentHealth -= damageTaken;
        SetHealthBar(currentHealth /maxHealth);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void SetHealthBar(float percentage)
    {
        if (healthBar != null)
        {
            healthBar.GetComponent<Image>().fillAmount = percentage;
        }
    }
}
