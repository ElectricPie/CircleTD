using UnityEngine;

[RequireComponent(typeof(TowerAttack))]
public class TowerAttackUpgrades : MonoBehaviour
{
    [SerializeField] private float m_upgradeMultiplier = 1.1f;
    [SerializeField] private float m_costMultiplier = 1.15f;

    private TowerAttack m_towerAttack;
    
    private int m_damageLevel = 0;
    private int m_speedLevel = 0;

    private void Awake()
    {
        m_towerAttack = GetComponent<TowerAttack>();
    }

    public void UpgradeDamage()
    {
        float newDamage = m_towerAttack.CurrentDamage;
        newDamage *= m_upgradeMultiplier;

        m_towerAttack.CurrentDamage = newDamage;
    }

    public void UpgradeSpeed()
    {
        
    }
}
