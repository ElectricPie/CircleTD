using UnityEngine;

public class AbilityInitData
{
    public GameObject Owner { get; } 
    
    private AbilityInitData() {}
    public AbilityInitData(GameObject owner)
    {
        Owner = owner;
    }
}

public class AbilityInstance
{
    public int Level { get; private set; } = 1;
    
    protected readonly AbilityData AbilityData;
    protected readonly GameObject Owner;
    
    private AbilityInstance() { }
    public AbilityInstance(AbilityData abilityData, AbilityInitData initData)
    {
        AbilityData = abilityData;
        Owner = initData.Owner;
    }
    
    public virtual bool TryActivate(GameObject target = null)
    {
        return AbilityData.Execute(target, Owner, Level);
    }

    public virtual void SetLevel(int newLevel)
    {
        Level = Mathf.Max(1, newLevel);
    }
}
