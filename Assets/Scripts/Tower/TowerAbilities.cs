using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerAbilities : MonoBehaviour
{
    [SerializeField] private GameObject m_owningPlayer;
    [SerializeField] private TowerWaves m_towerWaves;
    
    [SerializeField] private TowerBasicAttackAbilityData m_basicAttack;

    [SerializeField] private Vector3 m_projectileSpawnPointOffset;

    [SerializeField] private AbilityData DEBUG_poisonTipAbility;

    public TowerBasicAttackAbilityInstance BasicAttackInstance { get; private set; }

    private readonly HashSet<AbilityInstance> m_onHitAbilities = new HashSet<AbilityInstance>();

    public void AddOnHitAbility(AbilityData newAbility)
    {
        AbilityInitData newAbilityData = new AbilityInitData(m_owningPlayer);
        m_onHitAbilities.Add(newAbility.CreateAbilityInstance(newAbilityData));
    }
    
    private void Awake()
    {
        if (m_basicAttack == null)
            throw new Exception($"{name} is missing Basic Attack ability");
        
        TowerBasicAttackInitData initData = new TowerBasicAttackInitData(m_owningPlayer)
        {
            SpawnTransform = transform,
            SpawnOffSet = m_projectileSpawnPointOffset,
            TowerWaveComponent = m_towerWaves
        };
        BasicAttackInstance = (TowerBasicAttackAbilityInstance)m_basicAttack.CreateAbilityInstance(initData);
        BasicAttackInstance.OnTargetHit += target =>
        { 
            // Activate any other abilities 
            foreach (AbilityInstance ability in m_onHitAbilities)
            {
                ability.TryActivate(target);
            }
        };
        
        // Invoke(nameof(DEBUG_ADD_POISON), 5.0f);
    }

    private void DEBUG_ADD_POISON()
    {
        AddOnHitAbility(DEBUG_poisonTipAbility);
    }

    private void Update()
    {
        BasicAttackInstance.TryActivate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + m_projectileSpawnPointOffset, 0.5f);
    }
}