using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability Data", menuName = "Abilities/New Ability Data")]
public class AbilityData : ScriptableObject
{
    public string Label;
    [SerializeReference] public List<GameEffectScriptableObject> Effects;

    protected void OnEnable()
    {
        if (string.IsNullOrEmpty(Label))
        {
            Label = name;
        }

        Effects ??= new List<GameEffectScriptableObject>();
    }
}