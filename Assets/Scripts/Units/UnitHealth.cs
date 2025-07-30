using System;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    [SerializeField] [Min(0)] private float m_maxHealth = 20.0f;
    
    public event Action<float> OnUnitCurrentHealthChangedEvent = delegate { };
    public event Action<float> OnUnitMaxHealthChangedEvent = delegate { };
    public event Action<GameObject> OnKilledEvent = delegate { };
    
    public float CurrentHealth { get; private set; }
    public float MaxHealth => m_maxHealth;
    
    
    protected void Awake()
    {
        CurrentHealth = m_maxHealth;
        
        OnUnitMaxHealthChangedEvent?.Invoke(CurrentHealth);
        OnUnitCurrentHealthChangedEvent?.Invoke(m_maxHealth);
    }
    
    public void Damage(float damageAmount)
    {
        // Clamp the damage to a minimum of 1
        damageAmount = damageAmount < 1 ? 1 : damageAmount;
        
        CurrentHealth -= damageAmount;

        OnUnitCurrentHealthChangedEvent?.Invoke(CurrentHealth);

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
        
        OnUnitMaxHealthChangedEvent?.Invoke(m_maxHealth);
        OnUnitCurrentHealthChangedEvent?.Invoke(CurrentHealth);
    }
    
    private void UnitKilled()
    {
        OnKilledEvent?.Invoke(gameObject);
        
        Destroy(gameObject);
    }
}