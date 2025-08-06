using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerAbilities : MonoBehaviour
{
    [SerializeField] private GameObject m_owningPlayer;
    [SerializeField] private TowerWaves m_towerWaves;
    
    [SerializeField] private TowerBasicAttackAbilityData m_basicAttack;
    // [SerializeField] private List<AbilityData> m_activeAbilities;
    // private List<AbilityInstance> m_activeAbilityInstances;

    [SerializeField] private Vector3 m_projectileSpawnPointOffset;

    public TowerBasicAttackAbilityInstance BasicAttackInstance { get; private set; }

    private void Awake()
    {
        if (m_basicAttack == null)
        {
            throw new Exception($"{name} is missing Basic Attack ability");
        }

        // m_activeAbilityInstances = new List<AbilityInstance>();

        BasicAttackInstance = new TowerBasicAttackAbilityInstance();
        TowerBasicAttackInitData initData = new TowerBasicAttackInitData(m_owningPlayer, m_basicAttack)
        {
            SpawnTransform = transform,
            SpawnOffSet = m_projectileSpawnPointOffset,
            TowerWaveComponent = m_towerWaves
        };
        BasicAttackInstance.InitAbility(initData);
    }

    private void Update()
    {
        BasicAttackInstance.TryActivate();
        
        // foreach (AbilityInstance instance in m_activeAbilityInstances)
        // {
        //     instance.TryActivate();
        // }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + m_projectileSpawnPointOffset, 0.5f);
    }
}