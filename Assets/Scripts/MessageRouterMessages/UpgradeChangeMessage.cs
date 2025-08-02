public struct UpgradeChangeMessage
{
    public float NewCost;
    public float NewValue;
    public float NextValue;

    public UpgradeChangeMessage(float newCost, float newValue, float nextValue)
    {
        NewCost = newCost;
        NewValue = newValue;
        NextValue = nextValue;
    }
}