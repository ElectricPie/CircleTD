using System;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [SerializeField] private TowerWaves m_towerWaves;

    // TODO: Remove after testing
    [SerializeField] private Unit m_curretTarget;

    private void Start()
    {
        InvokeRepeating(nameof(GetNextTarget), 0.0f, 20.0f);
    }

    private void GetNextTarget()
    {
        if (m_towerWaves == null)
        {
            Debug.Log($"{this.name} is missing reference to its TowerWaves script");
            return;
        }

        m_curretTarget = m_towerWaves.GetOldestUnit();
    }
}
