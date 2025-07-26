using CircleTD.Messages;
using ElectricPie.UnityMessageRouter;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLivesManager : MonoBehaviour
{
    [SerializeField] private int m_initialLives = 40;
    
    [Header("Message Router Channels")]
    [SerializeField] private string m_livesChangedChannel = "LivesChanged";
    
    private int m_maxLives;
    private int m_currentLives;

    private void Awake()
    {
        m_maxLives = m_initialLives;
        m_currentLives = m_initialLives;
    }

    public void OnUnitsSpawned(Unit spawnedUnit)
    {
        if (spawnedUnit is null)
        {
            return;
        }

        UnitLiveCost cost = spawnedUnit.GetComponent<UnitLiveCost>();
        if (cost is not null)
        {
            m_currentLives -= cost.LiveCost;
        }

        MessageRouter.Broadcast(m_livesChangedChannel, new LiveChangedMessage(m_currentLives, m_maxLives));
    }

    public void OnUnitKilled(Unit killedUnit)
    {
        if (killedUnit is null)
        {
            return;
        }
        
        UnitLiveCost cost = killedUnit.GetComponent<UnitLiveCost>();
        if (cost is not null)
        {
            m_currentLives += cost.LiveCost;
        }

        MessageRouter.Broadcast(m_livesChangedChannel, new LiveChangedMessage(m_currentLives, m_maxLives));
    }
}
