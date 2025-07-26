using UnityEngine;

[RequireComponent(typeof(PlayerLivesManager), typeof(PlayerMoney))]
public class PlayerManager : MonoBehaviour
{
    public PlayerLivesManager LivesManager { get; private set; }
    public PlayerMoney MoneyManager { get; private set; }

    private void Awake()
    {
        LivesManager = GetComponent<PlayerLivesManager>();
        MoneyManager = GetComponent<PlayerMoney>();
    }
}