using System;
using UnityEngine;

[Serializable]
public abstract class GameEffect
{
    public abstract void Execute(GameObject caster, GameObject target, int level = 1);

    public object Clone()
    {
        return MemberwiseClone();
    }
}

[CreateAssetMenu(fileName = "New Effect", menuName = "Abilities/New Effect")]
public class GameEffectScriptableObject : ScriptableObject
{
    [SerializeReference] private GameEffect m_effect;

    public GameEffect Effect => m_effect;
}