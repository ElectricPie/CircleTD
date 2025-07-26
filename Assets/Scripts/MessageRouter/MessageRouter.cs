using System;
using System.Collections.Generic;

namespace ElectricPie.UnityMessageRouter
{
    public static class MessageRouter
    {
        private static readonly Dictionary<string, HashSet<IMessageRouterHandle>> m_messageHandlers =
            new Dictionary<string, HashSet<IMessageRouterHandle>>();

        private static readonly HashSet<IMessageRouterHandle> m_handlesToRemove = new HashSet<IMessageRouterHandle>();

        /// <summary>
        /// Registers a callback for a specific message type on a specific channel.
        /// </summary>
        /// <param name="owner">The object that is listening for the message</param>
        /// <param name="chanel">The name of the chanel to listen to</param>
        /// <param name="callback">The method callback</param>
        /// <typeparam name="TPayloadType">The object which will be used as the payload</typeparam>
        /// <exception cref="ArgumentNullException">If owner or callback are invalid</exception>
        public static void Register<TPayloadType>(object owner, string chanel, Action<TPayloadType> callback)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner), "Owner cannot be null");

            if (callback == null)
                throw new ArgumentNullException(nameof(callback), "Callback cannot be null");

            MessageRouterHandle<TPayloadType> handler = new MessageRouterHandle<TPayloadType>(owner, chanel, callback);
            if (!m_messageHandlers.ContainsKey(chanel))
            {
                m_messageHandlers[chanel] = new HashSet<IMessageRouterHandle>();
            }

            m_messageHandlers[chanel].Add(handler);
        }

        /// <summary>
        /// Unregisters a handle from the specified channel.
        /// </summary>
        /// <param name="handle">The handle to be unregistered</param>
        /// <param name="chanel">The chanel to be unregistered from</param>
        public static void Unregister(IMessageRouterHandle handle, string chanel)
        {
            if (m_messageHandlers.TryGetValue(chanel, out HashSet<IMessageRouterHandle> messageHandlers))
            {
                messageHandlers.Remove(handle);
            }
        }

        /// <summary>
        /// Unregisters a handle from all channels.
        /// </summary>
        /// <param name="handle">The handle to be unregistered</param>
        public static void Unregister(IMessageRouterHandle handle)
        {
            foreach (string channel in m_messageHandlers.Keys)
            {
                Unregister(handle, channel);
            }
        }

        /// <summary>
        /// Sends a message to all registered objects on the specified channel.
        /// </summary>
        /// <param name="chanel">The chanel to broadcast on</param>
        /// <param name="message">The payload to broadcast</param>
        /// <typeparam name="TPayloadType">The object to be used as payload</typeparam>
        public static void Broadcast<TPayloadType>(string chanel, TPayloadType message)
        {
            if (m_messageHandlers.TryGetValue(chanel, out HashSet<IMessageRouterHandle> messageHandlers))
            {
                foreach (IMessageRouterHandle handler in messageHandlers)
                {
                    // Check if the owner is still valid (regular object || UnityEngine.Object)
                    if (handler.Owner == null ||
                        (handler.Owner is UnityEngine.Object unityObject && unityObject == null))
                    {
                        m_handlesToRemove.Add(handler);
                        continue;
                    }

                    handler.Invoke(message);
                }

                if (m_handlesToRemove.Count <= 0)
                    return;

                // Remove handlers that are no longer valid
                foreach (IMessageRouterHandle handler in m_handlesToRemove)
                {
                    Unregister(handler, chanel);
                }

                m_handlesToRemove.Clear();
            }
        }
    }
}