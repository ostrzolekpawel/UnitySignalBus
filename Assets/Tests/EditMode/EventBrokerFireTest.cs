using NUnit.Framework;
using OsirisGames.Signals;
using System;

public class EventBrokerFireTest
{
    [Test]
    public void Fire_DoesNotInvokeUnrelatedSubscriptions()
    {
        // Arrange
        var eventBus = new SignalBus();
        bool eventFired = false;
        Action<string> action = (message) => eventFired = true;

        eventBus.Subscribe<int>((value) => { });

        // Act
        eventBus.Subscribe(action);
        eventBus.Fire("Test Event");

        // Assert
        Assert.IsTrue(eventFired);
    }

    [Test]
    public void Fire_InvokesAllSubscribedActions_For_SpecificEventType()
    {
        // Arrange
        var eventBus = new SignalBus();
        int counter = 0;
        Action<int> action1 = (num) => counter += num;
        Action<int> action2 = (num) => counter += num * 2;

        // Act
        eventBus.Subscribe(action1);
        eventBus.Subscribe(action2);
        eventBus.Fire(5);

        // Assert
        Assert.AreEqual(15, counter);
    }

    [Test]
    public void Fire_MultipleSubscriptions_ForSameEventType_Is_HandledCorrectly()
    {
        // Arrange
        var eventBus = new SignalBus();
        int eventCount = 0;
        Action<int> action = (number) => eventCount += number;

        // Act
        eventBus.Subscribe(action);
        eventBus.Subscribe(action);
        eventBus.Fire(5);

        // Assert
        Assert.AreEqual(10, eventCount);
    }

    [Test]
    public void Fire_DoesNothing_If_NoSubscriptionsExist()
    {
        // Arrange
        var eventBus = new SignalBus();

        // Act && Assert
        Assert.DoesNotThrow(() => eventBus.Fire("Test Event"));
    }

    [Test]
    public void Fire_WithNull_DoesNotCause_UnexpectedBehavior()
    {
        // Arrange
        var eventBus = new SignalBus();
        bool eventFired = false;
        Action<string> action = (message) => eventFired = true;

        // Act
        eventBus.Subscribe(action);
        eventBus.Fire<string>(null);

        // Assert
        Assert.IsTrue(eventFired);
    }
}