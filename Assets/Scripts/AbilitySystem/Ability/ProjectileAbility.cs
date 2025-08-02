using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "New Projectile Ability Data", menuName = "Abilities/New Projectile Ability Data")]
public class ProjectileAbilityData : AbilityData
{
    [SerializeField] private GameEffectScriptableObject m_damageEffectObject;
    [SerializeField] private TowerProjectile m_projectilePrefab = null;
    [SerializeField] private int m_poolSize = 10;
    [SerializeField] [Min(0.1f)] private float m_fireRate = 1;

    public TowerProjectile ProjectilePrefab => m_projectilePrefab;
    public int PoolSize => m_poolSize;
    public float FireRate => m_fireRate;
    public GameEffectScriptableObject DamageGameEffect => m_damageEffectObject;
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
    public GameEffectScriptableObject DamageEffect { get; private set; }
    public List<GameEffectScriptableObject> Effects { get; } = new List<GameEffectScriptableObject>();
    public float FireRate { get; private set; } = 1.0f;

    private ObjectPool<TowerProjectile> m_projectilePool;
    private float m_projectileSpeed;

    private Transform m_spawnPoint;
    private Unit m_currentTarget;
    private TowerWaves m_waveComponent;

    private float m_lastFireTime = int.MinValue;
    
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

        m_projectileSpeed = projectileAbilityData.ProjectilePrefab.GetComponent<TowerProjectileMovement>().Speed;
        m_spawnPoint = projectileInitData.SpawnTransform;
        m_waveComponent = projectileInitData.TowerWaveComponent;
        FireRate = projectileAbilityData.FireRate;
        
        // Need to keep track of damage effect as it will be upgraded, need to clone it otherwise it will affect original
        DamageEffect = projectileAbilityData.DamageGameEffect;
        Effects.Add(DamageEffect);
        Effects.AddRange(projectileAbilityData.Effects);

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
        
        if (m_lastFireTime + FireRate < Time.time)
        {
            m_lastFireTime = Time.time;
            m_projectilePool.Get();
        }
    }

    public float GetDamage(int level)
    {
        return ((DamageEffect)DamageEffect.Effect).DamageCurve.Evaluate(level);
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

    public List<GameEffectScriptableObject> GetEffects()
    {
        return Effects;
    }
}