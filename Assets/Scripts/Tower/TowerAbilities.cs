using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerAbilities : MonoBehaviour
{
    [SerializeField] private GameObject m_owningPlayer;
    [SerializeField] private TowerWaves m_towerWaves;
    
    [SerializeField] private AbilityData m_basicAttack;
    [SerializeField] private List<AbilityData> m_abilities;
    private List<AbilityInstance> m_abilityInstances;

    [SerializeField] private Vector3 m_projectileSpawnPointOffset;
    
    private void Awake()
    {
        if (m_basicAttack == null)
        {
            throw new Exception($"{name} is missing Basic Attack ability");
        }

        m_abilityInstances = new List<AbilityInstance>();

        ProjectileAbilityInstance basicAttack = new ProjectileAbilityInstance();
        ProjectileInitData initData = new ProjectileInitData(m_owningPlayer, m_basicAttack)
        {
            SpawnTransform = transform,
            SpawnOffSet = m_projectileSpawnPointOffset,
            TowerWaveComponent = m_towerWaves
        };
        basicAttack.InitAbility(initData);
        m_abilityInstances.Add(basicAttack);
    }

    private void Update()
    {
        foreach (AbilityInstance instance in m_abilityInstances)
        {
            instance.TryActivate();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + m_projectileSpawnPointOffset, 0.5f);
    }
}