using System.Collections;
using UnityEngine;

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
    private float m_projectileSpeed = 0.0f;
    
    private void Start()
    {
        CurrentDamage = m_baseDamage;
        CurrentSpeed = m_defaultSpeed;

        if (m_projectilePrefab is not null)
        {
            m_projectileSpeed = m_projectilePrefab.GetComponent<TowerProjectile>().Speed;
        }

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
            
            Vector3 predictedPos = GetPredictedLocation(m_currentTarget.transform.position);
            
            // Create projectile
            Vector3 spawnPoint = transform.position + m_projectileSpawnPoint;
            TowerProjectile projectile = Instantiate(m_projectilePrefab, spawnPoint, Quaternion.identity).GetComponent<TowerProjectile>();
            projectile.SetupProjectile(CurrentDamage, m_currentTarget, predictedPos);
            
            yield return new WaitForSeconds(1 / CurrentSpeed);
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
}
