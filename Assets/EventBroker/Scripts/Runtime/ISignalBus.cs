using System;

namespace OsirisGames.Signals
{
    public interface ISignalBus
    {
        void Subscribe<T>(Action<T> action);
        void Fire<T>(T signal);
        void Unsubscribe<T>(Action<T> action);
    }
}
