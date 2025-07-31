using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TowerProjectileMovement))]
public class TowerProjectile : MonoBehaviour
{
    public event Action<TowerProjectile> OnHitEvent = delegate { };
    public event Action<TowerProjectile> OnTimeoutEvent = delegate { };
    public event Action<TowerProjectile> OnTargetKilledEvent = delegate { };
    public ISharedEffects SharedEffects;

    [Tooltip("Time after creation before projectile the projectile triggers its on hit event")]
    [SerializeField] [Min(0)] private float m_timeoutTime = 4.0f;

    private TowerProjectileMovement m_movementComponent = null;
    private GameObject m_target = null;

    private void Awake()
    {
        m_movementComponent = GetComponent<TowerProjectileMovement>();
    }

    public void SetTarget(Unit target, Vector3 targetPos)
    {
        if (target == null)
        {
            OnTimeoutEvent?.Invoke(this);
            return;
        }

        target.HealthComponent.OnKilledEvent += OnTargetKilled;

        m_movementComponent.TargetPos = targetPos;
        m_target = target.gameObject;
        StartTimeout();
    }

    private void StartTimeout()
    {
        CancelInvoke(nameof(Timeout));
        Invoke(nameof(Timeout), m_timeoutTime);
    }

    private void Timeout()
    {
        ApplyEffects();
        m_target = null;
        OnTimeoutEvent?.Invoke(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != m_target.gameObject) 
            return;
        
        CancelInvoke(nameof(Timeout));
        ApplyEffects();
        OnHitEvent?.Invoke(this);
    }

    private void ApplyEffects()
    {
        if (SharedEffects == null || m_target == null)
            return;

        foreach (GameEffect effect in SharedEffects.GetEffects())
        {
            effect.Execute(gameObject, m_target.gameObject);
        }
    }

    private void OnTargetKilled(GameObject target)
    {
        CancelInvoke(nameof(Timeout));
        
        m_target = null;
        OnTargetKilledEvent?.Invoke(this);
    }
}