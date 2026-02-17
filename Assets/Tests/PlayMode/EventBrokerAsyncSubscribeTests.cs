using Cysharp.Threading.Tasks;
using NUnit.Framework;
using OsirisGames.Signals;
using System;
using System.Collections;
using UnityEngine.TestTools;

public class EventBrokerAsyncSubscribeTests
{
    [UnityTest]
    public IEnumerator Subscribe_AddsNewSubscription_For_EventType() => UniTask.ToCoroutine(async () =>
    {
        // Arrange
        var eventBus = new SignalBusAsync();
        bool eventFired = false;
        Func<string, UniTask> action = (message) =>
        {
            eventFired = true;
            return UniTask.FromResult(message);
        };

        // Act
        eventBus.Subscribe(action);
        await eventBus.FireAsync("Test Event");

        // Assert
        Assert.IsTrue(eventFired);
    });

    [Test]
    public void Subscribe_WithNull_ThrowsException()
    {
        // Arrange
        var eventBus = new SignalBusAsync();

        // Act && Assert
        Assert.Throws<ArgumentNullException>(() => eventBus.Subscribe<string>(null));
    }

    [UnityTest]
    public IEnumerator Subscribe_SameActionMultipleTimes_ResultsIn_MultipleInvocations() => UniTask.ToCoroutine(async () =>
    {
        // Arrange
        var eventBus = new SignalBusAsync();
        int invocationCount = 0;
        Func<string, UniTask> action = (message) =>
        {
            invocationCount++;
            return UniTask.FromResult(message);
        };

        // Act
        eventBus.Subscribe(action);
        eventBus.Subscribe(action);
        eventBus.Subscribe(action);
        await eventBus.FireAsync("Test Event");

        // Assert
        Assert.AreEqual(3, invocationCount);
    });
}
