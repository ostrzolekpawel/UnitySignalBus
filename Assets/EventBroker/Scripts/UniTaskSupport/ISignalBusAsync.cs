#if EVENTBROKER_UNITASK_ENABLED
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace OsirisGames.Signals
{
    public interface ISignalBusAsync
    {
        void Subscribe<T>(Func<T, UniTask> action);
        UniTask FireAsync<T>(T signal, CancellationToken token = default);
        void Unsubscribe<T>(Func<T, UniTask> action);
    }
}
#endif