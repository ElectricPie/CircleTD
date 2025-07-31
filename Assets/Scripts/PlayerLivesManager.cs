using CircleTD.Messages;
using ElectricPie.UnityMessageRouter;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLivesManager : MonoBehaviour
{
    [SerializeField] private int m_initialLives = 40;

    [Header("Message Router Channels")] [SerializeField]
    private string m_livesChangedChannel = "LivesChanged";

    private int m_maxLives;
    private int m_currentLives;

    private void Awake()
    {
        m_maxLives = m_initialLives;
        m_currentLives = m_initialLives;
    }

    public void OnUnitsSpawned(Unit spawnedUnit)
    {
        spawnedUnit.HealthComponent.OnKilledEvent += OnUnitKilled;

        UnitLiveCost cost = spawnedUnit.LivesCostComponent;

        m_currentLives -= cost.LiveCost;

        MessageRouter.Broadcast(m_livesChangedChannel, new LiveChangedMessage(m_currentLives, m_maxLives));
    }

    public void OnUnitKilled(GameObject killedUnitGameObject)
    {
        UnitLiveCost cost = killedUnitGameObject.GetComponent<UnitLiveCost>();

        m_currentLives += cost.LiveCost;

        MessageRouter.Broadcast(m_livesChangedChannel, new LiveChangedMessage(m_currentLives, m_maxLives));
    }
}