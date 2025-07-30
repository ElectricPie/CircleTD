using System;
using UnityEngine;

[Serializable]
public abstract class GameEffect
{
    public abstract void Execute(GameObject caster, GameObject target);
}