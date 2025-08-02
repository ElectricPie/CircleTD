using UnityEngine;

public abstract class AbilityInstance
{
    public abstract void InitAbility(AbilityInitData initData);

    public abstract void TryActivate();
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