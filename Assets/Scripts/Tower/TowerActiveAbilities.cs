using System;
using UnityEngine;

public class TowerActiveAbilities : MonoBehaviour
{
    [SerializeField] [Min(0)] private int m_maxAbilities = 8;
    
    private TowerAbility[] m_abilities = Array.Empty<TowerAbility>();

    private void Awake()
    {
        m_abilities = new TowerAbility[m_maxAbilities];
    }
}
