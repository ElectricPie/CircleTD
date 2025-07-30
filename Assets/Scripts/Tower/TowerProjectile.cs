using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    public Action<TowerProjectile> OnHitEvent = null;
    public float Speed => m_speed;
    public ISharedEffects SharedEffects;

    [Tooltip("In distance per second")] [SerializeField] private float m_speed = 1.0f;
    [Tooltip("Time after creation before projectile the projectile triggers its on hit event")] [SerializeField] [Min(0)] private float m_timeoutTime = 4.0f;

    private Unit m_target = null;
    private Vector3 m_targetPos = Vector3.zero;
    
    public void SetTarget(Unit target, Vector3 targetPos)
    {
        m_target = target;
        m_targetPos = targetPos;
    }

    public void StartTimeout()
    {
        CancelInvoke();
        Invoke(nameof(Timeout), m_timeoutTime);
    }

    protected void Update()
    {
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        if (!m_target)
        {
            OnHitEvent?.Invoke(this);
            return;
        }

        transform.LookAt(m_targetPos);
        
        float moveDistance = m_speed * Time.deltaTime;
        transform.Translate(Vector3.forward * moveDistance);
    }

    private void Timeout()
    {
        ApplyEffects();
        OnHitEvent?.Invoke(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == m_target.gameObject)
        {
            ApplyEffects();
            OnHitEvent?.Invoke(this);
        }
    }
    
    private void ApplyEffects()
    {
        if (m_target == null || SharedEffects == null)
            return;
        
        foreach (GameEffect effect in SharedEffects.GetEffects())
        {
            effect.Execute(gameObject, m_target.gameObject);
        }
    }
}
