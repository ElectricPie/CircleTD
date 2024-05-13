using TMPro;
using UnityEngine;

public class MoneyTracker : MonoBehaviour
{
    [SerializeField] private TMP_Text m_moneyNumberText;
    
    public void OnMoneyChanged(float moneyAmount)
    {
        if (m_moneyNumberText is null)
        {
            Debug.LogError($"Money Tracker on {name} is missing reference to MoneyNumberText", this);
            return;
        }

        m_moneyNumberText.text = moneyAmount.ToString("0");
    }
}
