using UnityEngine;

public class Unit : MonoBehaviour
{
    public int Health
    {
        get
        {
            return m_currentHealth;
        }
    }

    [SerializeField] [Min(0)] private int m_initialHealth = 20;
    [SerializeField] private int m_currentHealth;

    public void Damage(int damageAmount)
    {
        // Clamp the damage to a minimum of 1
        if (damageAmount < 1)
        {
            damageAmount = 1;
        }
        m_currentHealth -= damageAmount;

        // Handle unit death
        if (Health <= 0)
        {
            UnitKilled();
        }
    }
    
    protected void Awake()
    {
        m_currentHealth = m_initialHealth;
    }

    private void UnitKilled()
    {
        Destroy(this.gameObject);
    }
}
