using System;
using UnityEngine;
using UnityEngine.Events;

public class UnitHealth : MonoBehaviour
{
    [SerializeField] [Min(0)] private float m_maxHealth = 20.0f;
    
    /// <summary>
    /// Called when ever the current health is changed passing the game object this component is on and the new current health
    /// </summary>
    public event Action<GameObject, float> OnUnitCurrentHealthChangedEvent = delegate { };
    /// <summary>
    /// Called when ever the max health is changed passing the game object this component is on and the new max health
    /// </summary>
    public event Action<GameObject, float> OnUnitMaxHealthChangedEvent = delegate { };
    /// <summary>
    /// Called when ever this takes damage passing the game object this component is on and the damage taken
    /// </summary>
    public event Action<GameObject, float> OnDamageTakenEvent = delegate { };

    /// <summary>
    /// The object that was killed and the object responsible for killing
    /// </summary>
    public event UnityAction<GameObject, GameObject> OnKilledEvent = delegate { };
    
    public float CurrentHealth { get; private set; }
    public float MaxHealth => m_maxHealth;
    
    
    protected void Awake()
    {
        CurrentHealth = m_maxHealth;
        
        OnUnitMaxHealthChangedEvent?.Invoke(gameObject, CurrentHealth);
        OnUnitCurrentHealthChangedEvent?.Invoke(gameObject, m_maxHealth);
    }
    
    public void Damage(float damageAmount, GameObject damageCauser)
    {
        // Clamp the damage to a minimum of 0
        damageAmount = damageAmount < 0 ? 0 : damageAmount;
        CurrentHealth -= damageAmount;

        OnDamageTakenEvent?.Invoke(gameObject, damageAmount);
        OnUnitCurrentHealthChangedEvent?.Invoke(gameObject, CurrentHealth);

        if (CurrentHealth > 0) 
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
        
        OnUnitMaxHealthChangedEvent?.Invoke(gameObject, m_maxHealth);
        OnUnitCurrentHealthChangedEvent?.Invoke(gameObject, CurrentHealth);
    }
}