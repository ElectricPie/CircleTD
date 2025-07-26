using CircleTD.Messages;
using ElectricPie.UnityMessageRouter;
using TMPro;
using UnityEngine;

public class MoneyTracker : MonoBehaviour
{
    [SerializeField] private TMP_Text m_moneyNumberText;
    
    [Header("Message Router Channels")]
    [SerializeField] private string m_moneyChangedChannel = "PlayerMoneyChanged";
    
    private void Awake()
    {
        MessageRouter.Register<MoneyUpdateMessage>(this,m_moneyChangedChannel, OnMoneyChanged);
    }

    public void OnMoneyChanged(MoneyUpdateMessage message)
    {
        if (m_moneyNumberText is null) 
        {
            Debug.LogError($"Money Tracker on {name} is missing reference to MoneyNumberText", this);
            return;
        }
        
        m_moneyNumberText.text = message.NewAmount.ToString("0");
    }
}