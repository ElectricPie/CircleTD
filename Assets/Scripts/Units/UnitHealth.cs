using System;
using UnityEngine;
using UnityEngine.Events;

public class UnitHealth : MonoBehaviour
{
    [SerializeField] [Min(0)] private float m_maxHealth = 20.0f;
    
    public event Action<float> OnUnitCurrentHealthChangedEvent = delegate { };
    public event Action<float> OnUnitMaxHealthChangedEvent = delegate { };
    /// <summary>
    /// The object that was killed and the object responsible for killing
    /// </summary>
    public event UnityAction<GameObject, GameObject> OnKilledEvent = delegate { };
    
    public float CurrentHealth { get; private set; }
    public float MaxHealth => m_maxHealth;
    
    
    protected void Awake()
    {
        CurrentHealth = m_maxHealth;
        
        OnUnitMaxHealthChangedEvent?.Invoke(CurrentHealth);
        OnUnitCurrentHealthChangedEvent?.Invoke(m_maxHealth);
    }
    
    public void Damage(float damageAmount, GameObject damageCauser)
    {
        // Clamp the damage to a minimum of 0
        damageAmount = damageAmount < 0 ? 0 : damageAmount;
        CurrentHealth -= damageAmount;

        OnUnitCurrentHealthChangedEvent?.Invoke(CurrentHealth);

        if (CurrentHealth >= 0) 
            return;
        
        // Handle unit death
        OnKilledEvent?.Invoke(gameObject, damageCauser);
        Destroy(gameObject);
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
}