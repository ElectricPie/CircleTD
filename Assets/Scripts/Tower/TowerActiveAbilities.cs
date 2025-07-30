using System;
using UnityEngine;

public class TowerActiveAbilities : MonoBehaviour
{
    [SerializeField] [Min(0)] private int m_maxAbilities = 8;
    
    private GameEffect[] m_abilities = Array.Empty<GameEffect>();

    private void Awake()
    {
        m_abilities = new GameEffect[m_maxAbilities];
    }
}
