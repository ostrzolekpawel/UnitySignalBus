using Cysharp.Threading.Tasks;
using NUnit.Framework;
using OsirisGames.Signals;
using System;
using System.Collections;
using UnityEngine.TestTools;

public class EventBrokeAsyncrUnsubscribeTests
{
    [UnityTest]
    public IEnumerator Unsubscribe_RemovesCorrectSubscription() => UniTask.ToCoroutine(async () =>
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
        eventBus.Unsubscribe(action);
        eventFired = false;
        await eventBus.FireAsync("Test Event");

        // Assert
        Assert.IsFalse(eventFired);
    });

    [Test]
    public void Unsubscribe_WithNull_ThrowsException()
    {
        // Arrange
        var eventBus = new SignalBusAsync();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => eventBus.Unsubscribe<string>(null));
    }

    [Test]
    public void Unsubscribe_ForNonExistentSubscription_DoesNot_CauseUnexpectedBehavior()
    {
        // Arrange
        var eventBus = new SignalBusAsync();
        Func<string, UniTask> action = (message) => UniTask.CompletedTask;

        // Act && Assert
        Assert.DoesNotThrow(() => eventBus.Unsubscribe(action));
    }


    [UnityTest]
    public IEnumerator Unsubscribe_RemovesOneInstance_Of_IdenticalAction() => UniTask.ToCoroutine(async () =>
    {
        // Arrange
        var eventBus = new SignalBusAsync();
        int eventCount = 0;
        Func<int, UniTask> action = (number) =>
        {
            eventCount++;
            return UniTask.FromResult(eventCount);
        };

        // Act
        eventBus.Subscribe(action);
        eventBus.Subscribe(action);
        eventBus.Subscribe(action);
        eventBus.Unsubscribe(action);
        await eventBus.FireAsync(5);

        // Assert
        Assert.AreEqual(2, eventCount);
    });
}
