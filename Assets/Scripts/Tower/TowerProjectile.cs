using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TowerProjectileMovement))]
public class TowerProjectile : MonoBehaviour
{
    public Action<TowerProjectile> OnHitEvent = null;
    public ISharedEffects SharedEffects;
    
    [Tooltip("Time after creation before projectile the projectile triggers its on hit event")]
    [SerializeField] [Min(0)] private float m_timeoutTime = 4.0f;
    
    private TowerProjectileMovement m_movementComponent = null;
    private Unit m_target = null;

    private void Awake()
    {
        m_movementComponent = GetComponent<TowerProjectileMovement>();
    }
    
    public void SetTarget(Unit target, Vector3 targetPos)
    {
        if (target == null)
        {
            OnHitEvent?.Invoke(this);
            return;
        }
        
        m_movementComponent.SetTarget(target, targetPos);
        m_target = target;
        StartTimeout();
    }

    private void StartTimeout()
    {
        CancelInvoke();
        Invoke(nameof(Timeout), m_timeoutTime);
    }
    
    private void Timeout()
    {
        ApplyEffects();
        OnHitEvent?.Invoke(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_target != null)
        {
            if (collision.gameObject == m_target.gameObject)
            {
                ApplyEffects();
            }
        }
        
        OnHitEvent?.Invoke(this);
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
