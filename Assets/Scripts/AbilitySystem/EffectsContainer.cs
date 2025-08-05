using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal struct PeriodicEffectContainer
{
    public readonly GameObject Caster;
    public readonly GameEffect Effect;
    public readonly GameObject Target;
    public readonly int Level;

    public PeriodicEffectContainer(GameObject caster, GameObject target, GameEffect effect, int level)
    {
        Caster = caster;
        Effect = effect;
        Target = target;
        Level = level;
    }
}

public class EffectsContainer : MonoBehaviour
{
    public void ApplyEffect(GameObject caster, GameEffect effect, int level = 1)
    {
        switch (effect.DurationPolicy)
        {
            case DurationPolicy.Instant:
                ApplyEffectInternal(caster, effect, level);
                break;
            case DurationPolicy.Periodic:
                PeriodicEffectContainer effectContainer = new PeriodicEffectContainer(caster, gameObject, effect, level);
                IEnumerator newTickCoroutine = PeriodicEffectCoroutine(effectContainer);
                StartCoroutine(newTickCoroutine);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ApplyEffectInternal(GameObject caster, GameEffect effect, int level)
    {
        effect.Execute(caster, gameObject, level);
    }

    private static IEnumerator PeriodicEffectCoroutine(PeriodicEffectContainer effectContainer)
    {
        float effectStartTime = Time.time;
        
        if (effectContainer.Effect.PeriodicEffectValues.TriggerOnApplication)
        {
            effectContainer.Effect.Execute(effectContainer.Caster, effectContainer.Target, effectContainer.Level);
        }
        
        while (true)
        {
            if (effectStartTime + effectContainer.Effect.PeriodicEffectValues.Duration < Time.time)
            {
                yield break;
            }
            
            yield return new WaitForSeconds(effectContainer.Effect.PeriodicEffectValues.Period);
            effectContainer.Effect.Execute(effectContainer.Caster, effectContainer.Target, effectContainer.Level);
        }
    }
}