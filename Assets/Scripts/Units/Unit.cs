using UnityEngine;

[RequireComponent(typeof(UnitHealth))]
[RequireComponent(typeof(UnitMoney))]
public class Unit : MonoBehaviour
{
    public UnitHealth HealthComponent { get; private set; }
    public UnitLiveCost LivesCostComponent { get; private set; }
    public UnitMoney MoneyComponent { get; private set; }
    
    private void Awake()
    {
        HealthComponent = GetComponent<UnitHealth>();
        LivesCostComponent = GetComponent<UnitLiveCost>();
        MoneyComponent = GetComponent<UnitMoney>();
    }
}
