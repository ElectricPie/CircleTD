public abstract class TowerAbility
{
    /// <summary>
    /// To be called when the ability is added to the tower.
    /// </summary>
    public virtual void OnAdd() { }

    /// <summary>
    /// What happens when the ability is activated.
    /// </summary>
    public virtual void Activate() { }
}