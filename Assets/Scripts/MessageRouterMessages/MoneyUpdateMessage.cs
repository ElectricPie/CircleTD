namespace CircleTD.Messages
{
    public struct MoneyUpdateMessage
    {
        public readonly float NewAmount;

        public MoneyUpdateMessage(float newAmount)
        {
            NewAmount = newAmount;
        }
    }
}