using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "ScriptableObject/TowerAbility")]
public class AbilityData : ScriptableObject
{
    public string Label;
    [SerializeReference] public List<GameEffect> Effects;

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(Label))
        {
            Label = name;
        }

        Effects ??= new List<GameEffect>();
    }
}