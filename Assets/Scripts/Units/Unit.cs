using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float Health { get; private set; }

    [SerializeField] [Min(0)] private int m_initialHealth = 20;
    [SerializeField] private HealthBar m_healtBar;
    [SerializeField] [Min(1)] private float m_moneyWorth = 1.0f;
    
    public event Action<Unit> OnUnitKilledEvent;
    public float MoneyWorth => m_moneyWorth;

    public void Damage(float damageAmount)
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
