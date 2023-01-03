using System.Collections.Concurrent;
using Timelogger.Core.Interfaces;

namespace Timelogger.Infrastructure.MessageBus
{
    public class InternalMessageBus : IInternalMessageBus
    {
        public ConcurrentDictionary<Type, List<object>> subscribers;

        private IInternalLogger internalLogger;

        public InternalMessageBus(IInternalLogger internalLogger)
        {
            this.internalLogger = internalLogger;
            subscribers = new ConcurrentDictionary<Type, List<object>>();
        }

        public void Publish<T>(T value) where T : IInternalMessage
        {
            var type = typeof(T);

            if (value == null)
            {
                internalLogger.LogInfo($"Null published of type {type}. Doing nothing");
                return;
            }

            if (subscribers.ContainsKey(type))
                internalLogger.LogInfo(value.ToString());
            else
                internalLogger.LogInfo(value.ToString() + " (no internal subscribers)");
            

            if (!subscribers.ContainsKey(type)) return;

            foreach (var actionOjbect in subscribers[type])
            {
                var action = (Action<T>)actionOjbect;
                action(value);
            }
        }

        public void Subscribe<T>(Action<T> action) where T : IInternalMessage
        {
            var type = typeof(T);

            if (!subscribers.ContainsKey(type))
                subscribers.TryAdd(type, new List<object>());

            var actionList = subscribers[type];
            if (actionList.Contains(action)) return;

            actionList.Add(action);
        }

        public void Unsubscribe<T>(Action<T> action) where T : IInternalMessage
        {
            var type = typeof(T);

            if (!subscribers.ContainsKey(type)) return;

            var actionList = subscribers[type];
            if (!actionList.Contains(action)) return;

            actionList.Remove(action);
        }

        public void Dispose()
        {
            subscribers.Clear();
            subscribers = null;
        }

        private void Try<T>(Action<T> action, T value)
        {
            try
            {
                action(value);
            }
            catch
            {
                //Suppress
            }
        }
    }
}
