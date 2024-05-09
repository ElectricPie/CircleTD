using TMPro;
using UnityEngine;

public class TowerUpgradeButton : MonoBehaviour
{
    [SerializeField] private TMP_Text m_upgradeAmountText;
    [SerializeField] private TMP_Text m_upgradeButtonText;

    public void UpdateText(float currentValue, float nextValue, float cost)
    {
        if (m_upgradeAmountText is not null)
        {
            m_upgradeAmountText.text = $"{currentValue:0.00} -> {nextValue:0.00}";
        }

        if (m_upgradeButtonText is not null)
        {
            m_upgradeButtonText.text = $"£{cost}";
        }
    }
}