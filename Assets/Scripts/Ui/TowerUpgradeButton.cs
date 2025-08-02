using ElectricPie.UnityMessageRouter;
using TMPro;
using UnityEngine;

public class TowerUpgradeButton : MonoBehaviour
{
    [SerializeField] private TMP_Text m_upgradeAmountText;
    [SerializeField] private TMP_Text m_upgradeButtonText;
    
    [Header("Message Router Channels")]
    [SerializeField] private string m_upgradeChangeChannel = "UpgradeChannel";

    private void Awake()
    {
        MessageRouter.Register<UpgradeChangeMessage>(this, m_upgradeChangeChannel, 
            payload => {
                m_upgradeAmountText.text = $"{payload.NewValue:0.00} -> {payload.NextValue:0.00}";
                m_upgradeButtonText.text = $"£{payload.NewCost}";
            });
    }
}