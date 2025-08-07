using UnityEngine;

public abstract class AbilityData : ScriptableObject
{
    public string Label;
    
    protected void OnEnable()
    {
        if (string.IsNullOrEmpty(Label))
        {
            Label = name;
        }
    }

    public virtual bool Execute(GameObject target, GameObject caster, int level = 1) { return true; }

    public virtual AbilityInstance CreateAbilityInstance(AbilityInitData initData)
    {
        return new AbilityInstance(this, initData);
    }
}