using System;
using ElectricPie.UnityMessageRouter;
using UnityEngine;

public class TowerAttackUpgrades : MonoBehaviour
{
    [SerializeField] private PlayerMoney m_playerMoney;
    
    [SerializeField] private float m_costMultiplier = 1.15f;
    [SerializeField] private float m_upgradeInitialCost = 5.0f;
    
    [SerializeField] private TowerUpgradeButton m_damageButton;
    [SerializeField] private TowerUpgradeButton m_speedButton;
    
    [Header("Message Router Channels")]
    [SerializeField] private string m_damageUpgradeChannel = "DamageUpgradeChannel";
    [SerializeField] private string m_speedUpgradeChannel = "SpeedUpgradeChannel";

    private TowerAbilities m_towerAbilities;

    private ProjectileAbilityInstance m_projectileAbility;

    private void Awake()
    {
        if (m_playerMoney == null)
        {
            throw new Exception($"TowerAttackUpgrade on {name} is missing reference to PlayerMoney script");
        }

        m_towerAbilities = GetComponent<TowerAbilities>();
        m_projectileAbility = m_towerAbilities.BasicAttackInstance;
    }

    private void Start()
    {
        BroadcastDamageValues(m_upgradeInitialCost);
        BroadcastFireRateValues(m_upgradeInitialCost);
    }

    public void UpgradeDamage()
    {
        // Cost is rounded up to remove any decimals and to ensure the cost always goes up
        float upgradeCost = Mathf.Ceil(m_upgradeInitialCost * Mathf.Pow(m_costMultiplier, m_projectileAbility.Level - 1));
        
        if (!m_playerMoney.RemoveMoney(upgradeCost) && UIErrorMessage.Instance != null)
        {
            UIErrorMessage.Instance.ShowError("Insignificant money for upgrade");
            return;
        }

        m_projectileAbility.SetLevel(m_projectileAbility.Level + 1);
        
        float nextUpgradeCost = Mathf.Ceil(m_upgradeInitialCost * Mathf.Pow(m_costMultiplier, m_projectileAbility.Level - 1));
        BroadcastDamageValues(nextUpgradeCost);
    }

    public void UpgradeSpeed()
    {
        // Cost is rounded up to remove any decimals and to ensure the cost always goes up
        float upgradeCost = Mathf.Ceil(m_upgradeInitialCost * Mathf.Pow(m_costMultiplier, m_projectileAbility.FireRateLevel - 1));
        
        if (!m_playerMoney.RemoveMoney(upgradeCost) && UIErrorMessage.Instance != null)
        {
            UIErrorMessage.Instance.ShowError("Insignificant money for upgrade");
            return;
        }

        m_projectileAbility.FireRateLevel++;
        
        float nextUpgradeCost = Mathf.Ceil(m_upgradeInitialCost * Mathf.Pow(m_costMultiplier, m_projectileAbility.FireRateLevel - 1));
        BroadcastFireRateValues(nextUpgradeCost);
    }

    private void BroadcastDamageValues(float upgradeCost)
    {
        UpgradeChangeMessage damageUpgradeMessage = new UpgradeChangeMessage(
            upgradeCost,
            m_projectileAbility.GetDamage(m_projectileAbility.Level), 
            m_projectileAbility.GetDamage(m_projectileAbility.Level + 1) 
            );
        MessageRouter.Broadcast(m_damageUpgradeChannel, damageUpgradeMessage);
    }

    private void BroadcastFireRateValues(float upgradeCost)
    {
        UpgradeChangeMessage speedUpgradeMessage = new UpgradeChangeMessage(
            upgradeCost,
            m_projectileAbility.GetFireRate(m_projectileAbility.FireRateLevel), 
            m_projectileAbility.GetFireRate(m_projectileAbility.FireRateLevel + 1)
            );
        MessageRouter.Broadcast(m_speedUpgradeChannel, speedUpgradeMessage);
    }
}
