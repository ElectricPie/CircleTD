using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Unit : MonoBehaviour
{
    [SerializeField] [Min(0)] private float m_maxHealth = 20;
    public float CurrentHealth { get; private set; }

    [SerializeField] private HealthBar m_healtBar;
    
    public event Action<Unit> OnUnitKilledEvent;
    [Min(1)] public float MoneyWorth = 1.0f;
    
    
    public void Damage(float damageAmount)
    {
        // Clamp the damage to a minimum of 1
        if (damageAmount < 1)
        {
            damageAmount = 1;
        }
        CurrentHealth -= damageAmount;

        if (m_healtBar is not null)
        {
            m_healtBar.UpdateHealthBar(CurrentHealth, m_maxHealth);
        }

        // Handle unit death
        if (CurrentHealth <= 0)
        {
            UnitKilled();
        }
    }

    /// <summary>
    /// Adjusts the units health to the 
    /// </summary>
    /// <param name="newMaxHealth"></param>
    /// <param name="keepHeathPercentage"></param>
    public void UpdateMaxHealth(float newMaxHealth, bool keepHeathPercentage = true)
    {
        float healthPercent = 1.0f;
        if (keepHeathPercentage)
        {
            healthPercent = CurrentHealth / m_maxHealth;
        }

        m_maxHealth = newMaxHealth;
        CurrentHealth = m_maxHealth * healthPercent;
    }
    
    protected void Awake()
    {
        CurrentHealth = m_maxHealth;
        
        if (m_healtBar is not null)
        {
            m_healtBar.UpdateHealthBar(CurrentHealth, m_maxHealth);
        }
    }

    private void UnitKilled()
    {
        OnUnitKilledEvent?.Invoke(this);
        
        Destroy(this.gameObject);
    }
}
