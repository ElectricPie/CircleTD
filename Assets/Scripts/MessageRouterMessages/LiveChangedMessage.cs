namespace CircleTD.Messages
{
    public struct LiveChangedMessage
    {
        public readonly int CurrentLives;
        public readonly int MaxLives;
        
        public LiveChangedMessage(int currentLives, int maxLives)
        {
            CurrentLives = currentLives;
            MaxLives = maxLives;
        }
    }
}