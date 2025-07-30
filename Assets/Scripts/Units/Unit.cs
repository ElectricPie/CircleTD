using UnityEngine;

[RequireComponent(typeof(UnitHealth))]
public class Unit : MonoBehaviour
{
    [SerializeField] private HealthBar m_healthBar;
    
    [Min(1)] public float MoneyWorth = 1.0f;
}
