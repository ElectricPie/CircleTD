using System;

namespace ElectricPie.UnityMessageRouter
{
    public readonly struct MessageRouterHandle<TMessageType> : IMessageRouterHandle,
        IEquatable<MessageRouterHandle<TMessageType>>
    {
        public object Owner { get; }

        private readonly Action<TMessageType> m_callback;

        public MessageRouterHandle(object owner, string channel, Action<TMessageType> callback)
        {
            Owner = owner ?? throw new ArgumentNullException(nameof(owner), "Owner cannot be null");
            m_callback = callback;
        }

        public void Invoke(object message)
        {
            if (message is TMessageType typedMessage)
            {
                m_callback?.Invoke(typedMessage);
            }
        }

        public bool Equals(MessageRouterHandle<TMessageType> other)
        {
            return Equals(m_callback, other.m_callback) && Equals(Owner, other.Owner);
        }

        public override bool Equals(object obj)
        {
            return obj is MessageRouterHandle<TMessageType> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(m_callback, Owner);
        }
    }
}