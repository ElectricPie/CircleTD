using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "New Basic Attack Ability Data", menuName = "Abilities/New Basic Attack Ability Data")]
public class TowerBasicAttackAbilityData : AbilityData
{
    [SerializeReference, BoxGroup("Base Attack")] private DamageEffect m_baseAttackEffect;
    [SerializeField] private TowerProjectile m_projectilePrefab = null;
    [SerializeField] private int m_poolSize = 10;
    [SerializeField, Tooltip("The amount of projectiles fired per second")] private AnimationCurve m_fireRateCurve;

    public DamageEffect BaseAttackEffect => m_baseAttackEffect;
    public TowerProjectile ProjectilePrefab => m_projectilePrefab;
    public int PoolSize => m_poolSize;
    public AnimationCurve FireRateCurve => m_fireRateCurve;

    public override AbilityInstance CreateAbilityInstance(AbilityInitData initData)
    {
        return new TowerBasicAttackAbilityInstance(this, initData);
    }
}

public class TowerBasicAttackInitData : AbilityInitData
{
    public Transform SpawnTransform = null;
    public Vector3 SpawnOffSet = Vector3.zero;
    public TowerWaves TowerWaveComponent = null;

    public TowerBasicAttackInitData(GameObject owner) : base(owner)
    {
    }
}

public class TowerBasicAttackAbilityInstance : AbilityInstance, ISharedEffects
{
    private readonly TowerBasicAttackAbilityData m_basicAttackAbilityData;

    public int FireRateLevel { get; set; } = 1;

    public event Action<GameObject> OnTargetHit = delegate { };
    
    private readonly ObjectPool<TowerProjectile> m_projectilePool;
    private readonly float m_projectileSpeed;
    private readonly TowerWaves m_waveComponent;
    private readonly List<GameEffect> m_effects = new List<GameEffect>();
    
    private Unit m_currentTarget;
    private float m_lastFireTime = int.MinValue;

    
    public TowerBasicAttackAbilityInstance(AbilityData abilityData, AbilityInitData initData) : base(abilityData, initData)
    {
        if (initData is not TowerBasicAttackInitData projectileInitData)
            throw new Exception("Tried to initialized projectile ability with non TowerBasicAttackInitData");
        
        if (abilityData is not TowerBasicAttackAbilityData attackAbilityData)
            throw new Exception("Tried to initialized projectile ability with non TowerBasicAttackAbilityData");
        
        if (attackAbilityData.ProjectilePrefab == null)
            throw new Exception("Projectile Ability Data is missing projectile prefab");
        
        m_basicAttackAbilityData = attackAbilityData;
        m_projectileSpeed = m_basicAttackAbilityData.ProjectilePrefab.GetComponent<TowerProjectileMovement>().Speed;
        m_waveComponent = projectileInitData.TowerWaveComponent;
        
        m_effects.Add(m_basicAttackAbilityData.BaseAttackEffect);
        
        m_projectilePool = new ObjectPool<TowerProjectile>(
            () =>
            {
                TowerProjectile projectile = Object.Instantiate(m_basicAttackAbilityData.ProjectilePrefab);
                projectile.Owner = projectileInitData.Owner;
                projectile.Effects = this;
                return projectile;
            },
            projectile =>
            {
                if (m_currentTarget == null)
                {
                    Debug.LogError($"Tried to get projectile with null target");
                    return;
                }
                
                projectile.gameObject.SetActive(true);
                
                Vector3 spawnPoint = projectileInitData.SpawnTransform.position + projectileInitData.SpawnOffSet;
                projectile.transform.position = spawnPoint;
                Vector3 predictedPos = GetPredictedLocation(m_currentTarget.transform.position, spawnPoint);
                projectile.SetTarget(m_currentTarget, predictedPos);
                
                projectile.Level = Level;
    
                projectile.OnHitEvent += OnProjectileHit;
                projectile.OnTimeoutEvent += OnProjectileHit;
                projectile.OnTargetKilledEvent += OnProjectileHit;
            },
            projectile =>
            {
                projectile.gameObject.SetActive(false);
    
                projectile.OnHitEvent -= OnProjectileHit;
                projectile.OnTimeoutEvent -= OnProjectileHit;
                projectile.OnTargetKilledEvent -= OnProjectileHit;
            },
            projectile => { Object.Destroy(projectile.gameObject); }, false, m_basicAttackAbilityData.PoolSize,
            m_basicAttackAbilityData.PoolSize * 2);
    }
    
    // Ignoring target for this ability
    public override bool TryActivate(GameObject target = null)
    {
        if (m_currentTarget == null)
        {
            m_currentTarget = m_waveComponent.GetOldestUnit();
            if (m_currentTarget == null)
                return false;
        }
        
        if (m_lastFireTime + (1 / GetFireRate(FireRateLevel)) < Time.time)
        {
            m_lastFireTime = Time.time;
            m_projectilePool.Get();
        }
		return true;
    }
    
    /* ISharedEffects Interface begin */ 
    public List<GameEffect> GetEffects()
    {
        return m_effects;
    }
    /* ISharedEffects Interface end */

    public float GetDamage(int level)
    {
        return m_basicAttackAbilityData.BaseAttackEffect.DamageCurve.Evaluate(level);
    }

    public float GetFireRate(int level)
    {
        return m_basicAttackAbilityData.FireRateCurve.Evaluate(level);
    }

    private Vector3 GetPredictedLocation(Vector3 targetCurrentPosition, Vector3 projectileSpawn)
    {
        float projectileDistanceToTarget = Vector3.Distance(targetCurrentPosition, projectileSpawn);

        float angularVelocity = m_waveComponent.CurrentWaveRpm * ((2 * Mathf.PI) / 60.0f);
        float timeToTarget = projectileDistanceToTarget / m_projectileSpeed;

        Vector3 centre = m_waveComponent.transform.position;
        // Keep it on one plane so don't need to handle the y-axis
        centre.y = targetCurrentPosition.y;
        
        // Calculate how much the unit will rotate in a frame
        float startingAngle = Mathf.Atan2(targetCurrentPosition.z - centre.z, targetCurrentPosition.x - centre.x);
        float angleMoved = angularVelocity * timeToTarget;
        float newAngle = startingAngle - angleMoved;

        float targetDistanceFromTower = Vector3.Distance(targetCurrentPosition, centre);
        // Calculate the predicted position
        float x = targetDistanceFromTower * Mathf.Cos(newAngle);
        float z = targetDistanceFromTower * Mathf.Sin(newAngle);

        // Need to account for the towers position
        return new Vector3(x + centre.x, centre.y, z + centre.z);
    }

    private void OnProjectileHit(TowerProjectile projectile)
    {
        OnTargetHit?.Invoke(projectile.Target);
        m_projectilePool.Release(projectile);
    }
}