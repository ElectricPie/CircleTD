using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerAttack : MonoBehaviour
{
    [SerializeField] private TowerWaves m_towerWaves;
    [SerializeField] private GameObject m_projectilePrefab;
    [SerializeField] private float m_defaultAttackSpeed = 1.0f;
    [Tooltip("How long to wait if there is no current target before attempting to get another")] [SerializeField] private float m_unitScanTime = 0.1f;
    [SerializeField] private int m_baseDamage = 1;
    [SerializeField] private Vector3 m_projectileSpawnPoint;

    // TODO: Remove SerializeField after testing
    [SerializeField] private Unit m_curretTarget;

    private float m_currentAttackSpeed;
    private IEnumerator m_attackCoroutine;

    private void Start()
    {
        m_currentAttackSpeed = m_defaultAttackSpeed;

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

        m_curretTarget = m_towerWaves.GetOldestUnit();
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
            if (m_curretTarget == null)
            {
                GetNextTarget();
                yield return new WaitForSeconds(m_unitScanTime);
                continue;
            }
            
            Debug.Log("Attack");
            Vector3 spawnPoint = transform.position + m_projectileSpawnPoint;
            TowerProjectile projectile = Instantiate(m_projectilePrefab, spawnPoint, Quaternion.identity).GetComponent<TowerProjectile>();
            projectile.SetupProjectile(m_baseDamage, m_curretTarget);
            yield return new WaitForSeconds(m_currentAttackSpeed);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + m_projectileSpawnPoint, 0.5f);
    }
}
