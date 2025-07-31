using UnityEngine;

[RequireComponent(typeof(UnitHealth))]
public class Unit : MonoBehaviour
{
    [SerializeField] private HealthBar m_healthBar;
    
    [Min(1)] public float MoneyWorth = 1.0f;
    
    public UnitHealth HealthComponent { get; private set; }
    public UnitLiveCost LivesCostComponent { get; private set; }
    
    private void Awake()
    {
        HealthComponent = GetComponent<UnitHealth>();
        LivesCostComponent = GetComponent<UnitLiveCost>();
    }
}
