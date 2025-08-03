using System;
using System.Collections.Generic;
using UnityEngine;

internal class PeriodicEffectContainer
{
    private readonly GameObject m_caster;
    private readonly GameEffect m_effect;
    private readonly GameObject m_target;
    private readonly int m_level;
    
    private float m_lastTriggered;

    private PeriodicEffectContainer() {}
    public PeriodicEffectContainer(GameObject caster, GameObject target, GameEffect effect, int level)
    {
        m_caster = caster;
        m_effect = effect;
        m_target = target;
        m_level = level;
        
        if (effect.PeriodicEffectValues.TriggerOnApplication)
        {
            m_lastTriggered = float.MinValue;
        }
        else
        {
            m_lastTriggered = Time.time + effect.PeriodicEffectValues.Period;
        }
    }

    public void TryExecute()
    {
        if (m_lastTriggered + m_effect.PeriodicEffectValues.Period < Time.time)
            return;

        m_lastTriggered = Time.time;
        m_effect.Execute(m_caster, m_target, m_level);
    }
}

public class EffectsContainer : MonoBehaviour
{
    private readonly List<PeriodicEffectContainer> m_effects = new List<PeriodicEffectContainer>();

    public void ApplyEffect(GameObject caster, GameEffect effect, int level = 1)
    {
        switch (effect.DurationPolicy)
        {
            case DurationPolicy.Instant:
                ApplyEffectInternal(caster, effect, level);
                break;
            case DurationPolicy.Periodic:
                m_effects.Add(new PeriodicEffectContainer(caster, gameObject, effect, level));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ApplyEffectInternal(GameObject caster, GameEffect effect, int level)
    {
        effect.Execute(caster, gameObject, level);
    }

    private void Update()
    {
        foreach (PeriodicEffectContainer effect in m_effects)
        {
            effect.TryExecute();
        }
    }
}