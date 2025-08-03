using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public abstract class GameEffect
{
    [SerializeField] 
    private DurationPolicy m_durationPolicy = DurationPolicy.Instant;

    [SerializeField, ShowIf("m_durationPolicy", DurationPolicy.Periodic), InlineProperty, HideLabel] 
    private PeriodicEffectValues m_periodicEffectValues;

    public DurationPolicy DurationPolicy => m_durationPolicy;
    public PeriodicEffectValues PeriodicEffectValues => m_periodicEffectValues;
    
    public abstract void Execute(GameObject caster, GameObject target, int level = 1);
}

[Serializable]
public class PeriodicEffectValues {
    [SerializeField, BoxGroup("Periodic"), MinValue(0)] 
    private float m_duration = 1.0f;
    [SerializeField, BoxGroup("Periodic"), MinValue(0)] 
    private float m_period = 1.0f;
    [SerializeField, BoxGroup("Periodic")]
    private bool m_triggerOnApplication;

    public float Duration => m_duration;
    public float Period => m_period;
    public bool TriggerOnApplication => m_triggerOnApplication;
}

public enum DurationPolicy
{
    Instant,
    Periodic
}

[CreateAssetMenu(fileName = "New Effect", menuName = "Abilities/New Effect")]
public class GameEffectScriptableObject : ScriptableObject
{
    [SerializeReference, InlineProperty, HideLabel] 
    private GameEffect m_effect;

    public GameEffect Effect => m_effect;
}