using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerAttack : MonoBehaviour
{
    [SerializeField] private TowerWaves m_towerWaves;
    [SerializeField] private GameObject m_projectilePrefab;
    [Tooltip("The number of projectiles fired per second")] [SerializeField] private float m_defaultSpeed = 1.0f;
    [SerializeField] private int m_baseDamage = 1;
    [SerializeField] private Vector3 m_projectileSpawnPoint;

    private Unit m_currentTarget;

    public float CurrentDamage { get; set; } = 1;
    public float CurrentSpeed { get; set; } = 1;
    private IEnumerator m_attackCoroutine;

    private void Start()
    {
        CurrentDamage = m_baseDamage;
        CurrentSpeed = m_defaultSpeed;

        m_attackCoroutine = AttackCoroutine();
        StartCoroutine(m_attackCoroutine);
    }

    private void GetNextTarget()
    {
        if (m_towerWaves == null)
        {
            Debug.LogError($"{this.name} TowerAttack component is missing reference to its TowerWaves script");
            return;
        }

        m_currentTarget = m_towerWaves.GetOldestUnit();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
    }

    private IEnumerator AttackCoroutine()
    {
        if (m_projectilePrefab == null)
        {
            Debug.LogError($"{this.name} TowerAttack component is missing projectile prefab", this);
            yield break;
        }

        while (true)
        {
            if (m_currentTarget == null)
            {
                GetNextTarget();
                yield return null;
                continue;
            }
            
            // Create projectile
            Vector3 spawnPoint = transform.position + m_projectileSpawnPoint;
            TowerProjectile projectile = Instantiate(m_projectilePrefab, spawnPoint, Quaternion.identity).GetComponent<TowerProjectile>();
            projectile.SetupProjectile(CurrentDamage, m_currentTarget);
            
            yield return new WaitForSeconds(1 / CurrentSpeed);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + m_projectileSpawnPoint, 0.5f);
    }
}
