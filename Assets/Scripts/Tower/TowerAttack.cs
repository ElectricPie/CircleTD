using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TowerAttack : MonoBehaviour, ISharedEffects
{
    public float ProjectileDamage = 1;
    [Tooltip("The number of projectiles fired per second")] public float FireRate = 5.0f;
    
    [SerializeField] private TowerWaves m_towerWaves;
    [SerializeField] private TowerProjectile m_projectilePrefab;
    [SerializeField] private int m_projectilePoolSize = 20;
    
    [SerializeField] private Vector3 m_projectileSpawnPoint;

    [SerializeReference] private List<GameEffect> m_projectileEffects = new List<GameEffect>();

    private Unit m_currentTarget;

    private IEnumerator m_attackCoroutine;
    private float m_projectileSpeed = 0.0f;

    private ObjectPool<TowerProjectile> m_projectilePool;

    /* ISharedEffect Implementation start */
    public List<GameEffect> GetEffects()
    {
        return m_projectileEffects;
    }
    /* ISharedEffect Implementation end */
    
    private void Start()
    {
        if (m_projectilePrefab == null)
        {
            throw new System.Exception($"{name} TowerAttack component is missing projectile prefab");
        }
        
        m_projectileSpeed = m_projectilePrefab.GetComponent<TowerProjectileMovement>().Speed;
        m_projectilePool = new ObjectPool<TowerProjectile>(
            () => {
                TowerProjectile projectile = Instantiate(m_projectilePrefab);
                projectile.SharedEffects = this;
                return projectile;
            }, 
            projectile =>
            {
                projectile.gameObject.SetActive(true);
                Vector3 spawnPoint = transform.position + m_projectileSpawnPoint;
                Vector3 predictedPos = GetPredictedLocation(m_currentTarget.transform.position);
                projectile.transform.position = spawnPoint;
                projectile.SetTarget(m_currentTarget, predictedPos);
                
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
            projectile => {
                Destroy(projectile.gameObject);
            }, false, m_projectilePoolSize, m_projectilePoolSize * 2);

        m_attackCoroutine = AttackCoroutine();
        StartCoroutine(m_attackCoroutine);
    }

    private void GetNextTarget()
    {
        if (m_towerWaves == null)
        {
            Debug.LogError($"{name} TowerAttack component is missing reference to its TowerWaves script");
            return;
        }

        m_currentTarget = m_towerWaves.GetOldestUnit();
    }

    private IEnumerator AttackCoroutine()
    {
        while (true)
        {
            if (m_currentTarget == null)
            {
                GetNextTarget();
                yield return null;
                continue;
            }

            m_projectilePool.Get();
            
            yield return new WaitForSeconds(1 / FireRate);
        }
    }

    private Vector3 GetPredictedLocation(Vector3 targetPos)
    {
        Vector3 towerPosition = transform.position;
        float distanceToTarget = Vector3.Distance(towerPosition, m_currentTarget.transform.position);

        float angularVelocity = (m_towerWaves.CurrentWaveRpm * 2 * Mathf.PI) / 60;
        float timeToTarget = distanceToTarget / m_projectileSpeed;

        // Keep it on one plane so don't need to handle the y axis
        towerPosition.y = targetPos.y;

        // Calculate how much the unit will rotate in a frame
        float startingAngle = Mathf.Atan2(targetPos.z - towerPosition.z, targetPos.x - towerPosition.x);
        float angleMoved = angularVelocity * timeToTarget;
        float newAngle = startingAngle - angleMoved;

        // Calculate the predicted position
        float x = distanceToTarget * Mathf.Cos(newAngle);
        float z = distanceToTarget * Mathf.Sin(newAngle);

        // Need to account for the towers position
        return new Vector3(x + towerPosition.x, targetPos.y, z + towerPosition.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + m_projectileSpawnPoint, 0.5f);
    }

    private void OnProjectileHit(TowerProjectile projectile)
    {
        m_projectilePool.Release(projectile);
    }
}