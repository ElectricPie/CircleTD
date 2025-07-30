using System;
using UnityEngine;

[Serializable]
public class DamageEffect : GameEffect
{
    public float DamageAmount = 1.0f;

    public override void Execute(GameObject caster, GameObject target)
    {
        target.GetComponent<Unit>()?.Damage(DamageAmount);
    }
}