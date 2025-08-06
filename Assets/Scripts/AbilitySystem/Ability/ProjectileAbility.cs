using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "New Projectile Ability Data", menuName = "Abilities/New Projectile Ability Data")]
public class ProjectileAbilityData : AbilityData
{
    [SerializeReference, BoxGroup("Base Attack")] private DamageEffect m_baseAttackEffect;
    [SerializeField] private TowerProjectile m_projectilePrefab = null;
    [SerializeField] private int m_poolSize = 10;
    [SerializeField, Tooltip("The amount of projectiles fired per second")] private AnimationCurve m_fireRateCurve;

    public DamageEffect BaseAttackEffect => m_baseAttackEffect;
    public TowerProjectile ProjectilePrefab => m_projectilePrefab;
    public int PoolSize => m_poolSize;
    public AnimationCurve FireRateCurve => m_fireRateCurve;
}

public class ProjectileInitData : AbilityInitData
{
    public Transform SpawnTransform = null;
    public Vector3 SpawnOffSet = Vector3.zero;
    public TowerWaves TowerWaveComponent = null;

    public ProjectileInitData(GameObject owner, AbilityData abilityData) : base(owner, abilityData)
    {
    }
}

public class ProjectileAbilityInstance : AbilityInstance, ISharedEffects
{
    public int FireRateLevel { get; set; } = 1;
    
    private DamageEffect m_damageEffect;
    
    private ObjectPool<TowerProjectile> m_projectilePool;
    private float m_projectileSpeed;

    private Transform m_spawnPoint;
    private Unit m_currentTarget;
    private TowerWaves m_waveComponent;

    private float m_lastFireTime = int.MinValue;

    private ProjectileAbilityData m_abilityData;

    private readonly List<GameEffect> m_effects = new List<GameEffect>();

    
    public override void InitAbility(AbilityInitData initData)
    {
        if (initData is not ProjectileInitData { AbilityData: ProjectileAbilityData projectileAbilityData } projectileInitData)
        {
            Debug.LogError("Tried to initialized projectile ability with non ProjectileInitData");
            return;
        }

        if (projectileAbilityData.ProjectilePrefab == null)
        {
            throw new Exception("Projectile Ability Data is missing projectile prefab");
        }

        m_abilityData = projectileAbilityData;
        m_projectileSpeed = projectileAbilityData.ProjectilePrefab.GetComponent<TowerProjectileMovement>().Speed;
        m_spawnPoint = projectileInitData.SpawnTransform;
        m_waveComponent = projectileInitData.TowerWaveComponent;

        m_effects.Add(m_abilityData.BaseAttackEffect);
        m_effects.AddRange(projectileAbilityData.Effects);

        m_projectilePool = new ObjectPool<TowerProjectile>(
            () =>
            {
                TowerProjectile projectile = Object.Instantiate(projectileAbilityData.ProjectilePrefab);
                projectile.Owner = projectileInitData.Owner;
                projectile.Effects = this;
                return projectile;
            },
            projectile =>
            {
                projectile.gameObject.SetActive(true);
                
                Vector3 spawnPoint = m_spawnPoint.position + projectileInitData.SpawnOffSet;
                projectile.transform.position = spawnPoint;
                Vector3 predictedPos = GetPredictedLocation(m_currentTarget.transform.position);
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
            projectile => { Object.Destroy(projectile.gameObject); }, false, projectileAbilityData.PoolSize,
            projectileAbilityData.PoolSize * 2);
    }

    public override void TryActivate()
    {
        if (m_currentTarget == null)
        {
            m_currentTarget = m_waveComponent.GetOldestUnit();
            if (m_currentTarget == null)
                return;
        }
        
        if (m_lastFireTime + (1 / GetFireRate(FireRateLevel)) < Time.time)
        {
            m_lastFireTime = Time.time;
            m_projectilePool.Get();
        }
    }
    
    /* ISharedEffects Interface begin */ 
    public List<GameEffect> GetEffects()
    {
        return m_effects;
    }
    /* ISharedEffects Interface end */

    public float GetDamage(int level)
    {
        return m_abilityData.BaseAttackEffect.DamageCurve.Evaluate(level);
    }

    public float GetFireRate(int level)
    {
        return m_abilityData.FireRateCurve.Evaluate(level);
    }

    private Vector3 GetPredictedLocation(Vector3 targetPos)
    {
        Vector3 spawnPos = m_spawnPoint.position;
        float distanceToTarget = Vector3.Distance(spawnPos, m_currentTarget.transform.position);

        float angularVelocity = (m_waveComponent.CurrentWaveRpm * 2 * Mathf.PI) / 60;
        float timeToTarget = distanceToTarget / m_projectileSpeed;

        // Keep it on one plane so don't need to handle the y-axis
        spawnPos.y = targetPos.y;

        // Calculate how much the unit will rotate in a frame
        float startingAngle = Mathf.Atan2(targetPos.z - spawnPos.z, targetPos.x - spawnPos.x);
        float angleMoved = angularVelocity * timeToTarget;
        float newAngle = startingAngle - angleMoved;

        // Calculate the predicted position
        float x = distanceToTarget * Mathf.Cos(newAngle);
        float z = distanceToTarget * Mathf.Sin(newAngle);

        // Need to account for the towers position
        return new Vector3(x + spawnPos.x, targetPos.y, z + spawnPos.z);
    }

    private void OnProjectileHit(TowerProjectile projectile)
    {
        m_projectilePool.Release(projectile);
    }
}