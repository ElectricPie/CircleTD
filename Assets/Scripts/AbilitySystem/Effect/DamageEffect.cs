using System;
using UnityEngine;

[Serializable]
public class DamageEffect : GameEffect
{
    public AnimationCurve DamageCurve;

    public override void Execute(GameObject caster, GameObject target, int level = 1)
    {
        float damage = DamageCurve.Evaluate(level);
        target.GetComponent<UnitHealth>()?.Damage(damage, caster);
    }
}