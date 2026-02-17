using System;
using System.Collections.Generic;
using System.Linq;

namespace OsirisGames.Signals
{
    public class SignalBus : ISignalBus
    {
        private readonly Dictionary<Type, List<Delegate>> _subscriptions = new Dictionary<Type, List<Delegate>>();

        public void Subscribe<T>(Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("Action to subscribe can't be null");
            }

            Type type = typeof(T);
            if (!_subscriptions.ContainsKey(type))
            {
                _subscriptions[type] = new List<Delegate>();
            }

            _subscriptions[type].Add(action);
        }

        public void Fire<T>(T signal)
        {
            Type type = typeof(T);
            if (_subscriptions.ContainsKey(type))
            {
                foreach (var subscription in _subscriptions[type].OfType<Action<T>>())
                {
                    subscription.Invoke(signal);
                }
            }
        }

        public void Unsubscribe<T>(Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("Action to unsubscribe can't be null");
            }

            Type type = typeof(T);
            if (_subscriptions.ContainsKey(type))
            {
                _subscriptions[type].Remove(action);
            }
        }
    }
}
