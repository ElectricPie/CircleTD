using UnityEngine;

[CreateAssetMenu(fileName = "New Poison Tip Ability Data", menuName = "Abilities/New Poison Tip Ability Data")]
public class DamageAbilityData : AbilityData
{
    [SerializeField] private DamageEffect m_damageEffect;

    public override bool Execute(GameObject target, GameObject caster, int level = 1)
    {
        if (target == null || caster == null)
            return false;

        EffectsContainer effectsContainer = target.GetComponent<EffectsContainer>();
        if (effectsContainer == null)
            return false;
        
        effectsContainer.ApplyEffect(caster, m_damageEffect, level);
		return true;
    }
}