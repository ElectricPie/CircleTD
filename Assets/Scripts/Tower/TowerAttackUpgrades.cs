using UnityEngine;

[RequireComponent(typeof(TowerAttack))]
public class TowerAttackUpgrades : MonoBehaviour
{
    [SerializeField] private float m_upgradeMultiplier = 1.1f;
    [SerializeField] private float m_costMultiplier = 1.15f;

    [SerializeField] private TowerUpgradeButton m_damageButton;
    [SerializeField] private TowerUpgradeButton m_speedButton;

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

        if (m_damageButton is not null)
        {
            m_damageButton.UpdateText(newDamage, newDamage * m_upgradeMultiplier, 10);
        }
    }

    public void UpgradeSpeed()
    {
        float newSpeed = m_towerAttack.CurrentSpeed;
        newSpeed *= m_upgradeMultiplier;

        m_towerAttack.CurrentSpeed = newSpeed;

        if (m_speedButton is not null)
        {
            m_speedButton.UpdateText(newSpeed, newSpeed * m_upgradeMultiplier, 10);
        }
    }
}
