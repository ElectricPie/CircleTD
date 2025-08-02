using UnityEngine;

public abstract class AbilityInstance
{
    public int Level { get; private set; } = 1;
    
    public abstract void InitAbility(AbilityInitData initData);
    public abstract void TryActivate();

    public virtual void SetLevel(int newLevel)
    {
        Level = Mathf.Max(1, newLevel);
    }
}

public class AbilityInitData
{
    public GameObject Owner { get; private set; } 
    public AbilityData AbilityData { get; private set; }
    
    private AbilityInitData() {}

    public AbilityInitData(GameObject owner, AbilityData abilityData)
    {
        Owner = owner;
        AbilityData = abilityData;
    }
}