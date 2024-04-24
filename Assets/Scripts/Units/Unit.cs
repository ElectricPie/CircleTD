using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int Health { get; private set; }

    [SerializeField] [Min(0)] private int m_initialHealth = 20;
    [SerializeField] private HealthBar m_healtBar;

    public event Action<Unit> OnUnitKilledEvent;

    public void Damage(int damageAmount)
    {
        // Clamp the damage to a minimum of 1
        if (damageAmount < 1)
        {
            damageAmount = 1;
        }
        Health -= damageAmount;

        if (m_healtBar is not null)
        {
            m_healtBar.UpdateHealthBar(Health, m_initialHealth);
        }

        // Handle unit death
        if (Health <= 0)
        {
            UnitKilled();
        }
    }
    
    protected void Awake()
    {
        Health = m_initialHealth;
        
        if (m_healtBar is not null)
        {
            m_healtBar.UpdateHealthBar(Health, m_initialHealth);
        }
    }

    private void UnitKilled()
    {
        OnUnitKilledEvent?.Invoke(this);
        
        Destroy(this.gameObject);
    }
}