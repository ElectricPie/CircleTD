using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class PeriodicEffectContainer
{
    public readonly GameEffect Effect;
    public event Action OnExpiration;
    
    private readonly GameObject m_caster;
    private readonly GameObject m_target;
    private readonly int m_level;
    private float ExpirationTime { get; set; }
    
    private PeriodicEffectContainer() { }
    public PeriodicEffectContainer(GameObject caster, GameObject target, GameEffect effect, int level, Action expirationCallback)
    {
        m_caster = caster;
        Effect = effect;
        m_target = target;
        m_level = level;

        RefreshDuration();
        OnExpiration = expirationCallback;
        
        if (Effect.PeriodicEffectValues.TriggerOnApplication)
        {
            Execute();
        }
    }

    public void RefreshDuration()
    {
        ExpirationTime = Time.time + Effect.PeriodicEffectValues.Duration;
    }

    public bool HasExpired()
    {
        if (Time.time < ExpirationTime) 
            return false;
        
        OnExpiration?.Invoke();
        return true;
    }

    public void Execute()
    {
        Effect.Execute(m_caster, m_target, m_level);
    }
}

public class EffectsContainer : MonoBehaviour
{
    private readonly Dictionary<Type, PeriodicEffectContainer> m_effects = new Dictionary<Type, PeriodicEffectContainer>();

    public void ApplyEffect(GameObject caster, GameEffect effect, int level = 1)
    {
        switch (effect.DurationPolicy)
        {
            case DurationPolicy.Instant:
                effect.Execute(caster, gameObject, level);
                break;
            case DurationPolicy.Periodic:
                SetupPeriodicEffect(caster, effect, level);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetupPeriodicEffect(GameObject caster, GameEffect effect, int level)
    {
        if (m_effects.ContainsKey(effect.GetType()))
        {
            m_effects[effect.GetType()].RefreshDuration();
            return;
        }
        
        PeriodicEffectContainer effectContainer = new PeriodicEffectContainer(caster, gameObject, effect, level,
            () =>
            {
                m_effects.Remove(effect.GetType());
            });
        m_effects.Add(effect.GetType(), effectContainer);
        IEnumerator newTickCoroutine = PeriodicEffectCoroutine(effectContainer);
        StartCoroutine(newTickCoroutine);
    }
    
    private static IEnumerator PeriodicEffectCoroutine(PeriodicEffectContainer effectContainer)
    {
        while (true)
        {
            if (effectContainer.HasExpired())
            {
				Debug.Log("Effect Expired");
                yield break;
            }
            
            yield return new WaitForSeconds(effectContainer.Effect.PeriodicEffectValues.Period);
            effectContainer.Execute();
        }
    }
}