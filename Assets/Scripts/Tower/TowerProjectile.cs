using System;
using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    [Tooltip("In distance per second")] [SerializeField] private float m_speed = 1.0f;
    [Tooltip("Time after creation before projectile is destroyed")] [SerializeField] [Min(0)] private float m_destoryTime = 10.0f;

    public float Speed => m_speed;
    
    private float m_damage = 1;
    private Unit m_target = null;
    private Vector3 m_targetPos = Vector3.zero;

    public void SetupProjectile(float damage, Unit target, Vector3 targetPos)
    {
        m_damage = damage;
        m_target = target;
        m_targetPos = targetPos;
    }

    protected void Start()
    {
        Invoke(nameof(Timeout), m_destoryTime);
    }

    protected void Update()
    {
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        if (m_target == null)
        {
            Destroy(gameObject);
            return;
        }

        // transform.LookAt(m_target.transform);
        transform.LookAt(m_targetPos);
        
        float moveDistance = m_speed * Time.deltaTime;
        transform.Translate(Vector3.forward * moveDistance);
    }

    private void Timeout()
    {
        if (m_target is not null)
        {
            m_target.Damage(m_damage);
        }
        
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == m_target.gameObject)
        {
            m_target.Damage(m_damage);
            Destroy(gameObject);   
        }
    }
}
