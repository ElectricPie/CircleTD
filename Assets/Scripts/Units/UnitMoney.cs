using UnityEngine;

[RequireComponent(typeof(UnitHealth))]
public class UnitMoney : MonoBehaviour
{
    [Min(1)] public float MoneyWorth = 1.0f;

    private void Awake()
    {
        gameObject.GetComponent<UnitHealth>().OnKilledEvent += (_, killer) =>
        {
            killer.GetComponent<PlayerMoney>().AddMoney(MoneyWorth);
        };
    }
}
