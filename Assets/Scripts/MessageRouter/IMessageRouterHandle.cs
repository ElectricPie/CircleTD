namespace ElectricPie.UnityMessageRouter
{
    public interface IMessageRouterHandle
    {
        public object Owner { get; }

        public void Invoke(object message);
    }
}