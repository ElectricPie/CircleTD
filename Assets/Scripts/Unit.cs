using UnityEngine;

public class Unit : MonoBehaviour
{
    public int Health { get; private set; }

    [SerializeField] [Min(0)] private int m_initialHealth = 20;

    public void Damage(int damageAmount)
    {
        // Clamp the damage to a minimum of 1
        if (damageAmount < 1)
        {
            damageAmount = 1;
        }
        Health -= damageAmount;

        // Handle unit death
        if (Health < 0)
        {
            UnitKilled();
        }
    }
    
    protected void Awake()
    {
        Health = m_initialHealth;
    }

    private void UnitKilled()
    {
        // TODO: Handle unit killed
    }
}
