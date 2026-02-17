# Unity Signal Bus

## Installation

There is several options to install this package:
- UPM
- directly in manifest

### Unity Package Manager

Open Unity Package Manager and go to **Add package from git URL...** and paste [https://github.com/ostrzolekpawel/UnitySignalBus.git?path=Assets/SignalBus](https://github.com/ostrzolekpawel/UnitySignalBus.git?path=Assets/SignalBus)

### Manifest
Add link to package from repository directly to manifest.json

**Example**
```json
{
    "dependencies": {
        // other packages
        // ...
        "com.osirisgames.eventbroker": "https://github.com/ostrzolekpawel/UnitySignalBus.git?path=Assets/SignalBus"
    }
}
```

## Introduction

Simple implementation of `Signal Bus` which allows sync and async calls:
- `EventBus` - sync calls
- `EventBusAsync` - async calls using [UniTask](https://github.com/Cysharp/UniTask)

Plugin also provides interfaces for custom implentations.

### IEventBus

```cs
public interface IEventBus
{
    void Subscribe<T>(Action<T> action);
    void Fire<T>(T signal);
    void Unsubscribe<T>(Action<T> action);
}
```


### IEventBusAsync

```cs
public interface IEventBusAsync
{
    void Subscribe<T>(Func<T, UniTask> action);
    UniTask FireAsync<T>(T signal, CancellationToken token = default);
    void Unsubscribe<T>(Func<T, UniTask> action);
}
```

## Sync calls

For projects which use assembly definitions add to references `OsirisGames.EventBroker.Core`

### Usage Example

First create instance of `EventBus` or your own implementation of `IEventBus` interface.

```cs
IEventBus _eventBus = new EventBus();
```

Create class which will be representation of event

```cs
public class RewardCollectAnimation
{
    public int Amount { get; }

    public RewardCollectAnimation(int amount)
    {
        Amount = amount;
    }
}
```

Subscribe to event

```cs
public class RewardCollection : IDisposable
{
    private readonly IEventBus _eventBus;

    public RewardCollection(IEventBus eventBus)
    {
        eventBus.Subscribe<RewardCollectAnimation>(CollectReward);
        _eventBus = eventBus;
    }

    private void CollectReward(RewardCollectAnimation caller)
    {
        // do some staff
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<RewardCollectAnimation>(CollectReward);
    }
}
```

Call event
```cs
public class RewardCaller
{
    private readonly IEventBus _eventBus;

    public RewardCaller(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public void Collect()
    {
        _eventBus.Fire(new RewardCollectAnimation(5));
    }
}

```

## Async calls

For projects which use assembly definitions add to references `OsirisGames.EventBroker.CoreAsync`
and also add to define symbols **`EVENTBROKER_UNITASK_ENABLED`**

### Usage Example

First create instance `EventBusAsync` or your own implementation of `IEventBusAsync` interface.

```cs
IEventBusAsync _eventBus = new EventBusAsync();
```

Create class which will be representation of event

```cs
public class RewardCollectAnimation
{
    public int Amount { get; }

    public RewardCollectAnimation(int amount)
    {
        Amount = amount;
    }
}
```

Subscribe to event

```cs
public class RewardCollection : IDisposable
{
    private readonly IEventBusAsync _eventBus;

    public RewardCollection(IEventBusAsync eventBus)
    {
        eventBus.Subscribe<RewardCollectAnimation>(CollectReward);
        _eventBus = eventBus;
    }

    private async UniTask CollectReward(RewardCollectAnimation caller)
    {
        // do some staff
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<RewardCollectAnimation>(CollectReward);
    }
}
```

Call event
```cs
public class RewardCaller
{
    private readonly IEventBusAsync _eventBus;

    public RewardCaller(IEventBusAsync eventBus)
    {
        _eventBus = eventBus;
    }

    public async UniTask Collect(CancellationToken token)
    {
        await _eventBus.FireAsync(new RewardCollectAnimation(5), token);
    }
}

```
