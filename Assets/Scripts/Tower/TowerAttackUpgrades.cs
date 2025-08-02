using UnityEngine;

public class TowerAttackUpgrades : MonoBehaviour
{
    [SerializeField] private PlayerMoney m_playerMoney;
    
    [SerializeField] private float m_upgradeMultiplier = 1.1f;
    [SerializeField] private float m_costMultiplier = 1.15f;
    [SerializeField] private float m_upgradeInitialCost = 5.0f;
    
    [SerializeField] private TowerUpgradeButton m_damageButton;
    [SerializeField] private TowerUpgradeButton m_speedButton;

    private TowerAttack m_towerAttack;
    
    private int m_damageLevel = 1;
    private int m_speedLevel = 1;

    private void Awake()
    {
        m_towerAttack = GetComponent<TowerAttack>();

        if (m_playerMoney is null)
        {
            Debug.LogError($"TowerAttackUpgrade on {name} is missing reference to PlayerMoney script", this);
        }
    }

    private void Start()
    {
        if (m_damageButton is not null)
        {
            m_damageButton.UpdateText(m_towerAttack.ProjectileDamage, m_towerAttack.ProjectileDamage * m_upgradeMultiplier, m_upgradeInitialCost);
        }
        
        if (m_speedButton is not null)
        {
            m_speedButton.UpdateText(m_towerAttack.FireRate, m_towerAttack.FireRate * m_upgradeMultiplier, m_upgradeInitialCost);
        }
    }

    public void UpgradeDamage()
    {
        // Cost is rounded up to remove any decimals and to ensure the cost always goes up
        float upgradeCost =  Mathf.Ceil(m_upgradeInitialCost * Mathf.Pow(m_costMultiplier, m_damageLevel));
        
        if (!m_playerMoney.RemoveMoney(upgradeCost) && UIErrorMessage.Instance is not null)
        {
            UIErrorMessage.Instance.ShowError("Insignificant money for upgrade");
            return;
        }
        
        float newDamage = m_towerAttack.ProjectileDamage;
        newDamage *= m_upgradeMultiplier;

        m_towerAttack.ProjectileDamage = newDamage;
        m_damageLevel++;
        
        if (m_damageButton is not null)
        {
            m_damageButton.UpdateText(newDamage, newDamage * m_upgradeMultiplier,  Mathf.Ceil(upgradeCost * m_costMultiplier));
        }
    }

    public void UpgradeSpeed()
    {
        // Cost is rounded up to remove any decimals and to ensure the cost always goes up
        float upgradeCost = Mathf.Ceil(m_upgradeInitialCost * Mathf.Pow(m_costMultiplier, m_speedLevel));

        if (!m_playerMoney.RemoveMoney(upgradeCost) && UIErrorMessage.Instance is not null)
        {
            UIErrorMessage.Instance.ShowError("Insignificant money for upgrade");
            return;
        }
        
        float newSpeed = m_towerAttack.FireRate;
        newSpeed *= m_upgradeMultiplier;

        m_towerAttack.FireRate = newSpeed;
        m_speedLevel++;

        if (m_speedButton is not null)
        {
            m_speedButton.UpdateText(newSpeed, newSpeed * m_upgradeMultiplier,  Mathf.Ceil(upgradeCost * m_costMultiplier));
        }
    }
}
