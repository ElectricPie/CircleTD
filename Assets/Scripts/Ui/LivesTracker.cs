using CircleTD.Messages;
using ElectricPie.UnityMessageRouter;
using TMPro;
using UnityEngine;

public class LivesTracker : MonoBehaviour
{
    [SerializeField] private TMP_Text m_livesText;
    
    [Header("Message Router Channels")]
    [SerializeField] private string m_livesChangedChannel = "LivesChanged";
    
    private void Awake()
    {
        MessageRouter.Register<LiveChangedMessage>(this, m_livesChangedChannel, OnLivesChanged);
    }

    public void OnLivesChanged(int currentLives, int maxLives)
    {
        if (m_livesText is null)
        {
            Debug.LogError($"LivesTracker on {name} is missing reference to LivesText", this);
            return;
        }

        m_livesText.text = $"{currentLives:00}/{maxLives:00}";
    }

    public void OnLivesChanged(LiveChangedMessage message)
    {
        if (m_livesText is null)
        {
            Debug.LogError($"LivesTracker on {name} is missing reference to LivesText", this);
            return;
        }

        m_livesText.text = $"{message.CurrentLives:00}/{message.MaxLives:00}";
    }
}
