using NUnit.Framework;
using OsirisGames.Signals;
using System;

public class EventBrokerUnsubscribeTests
{
    [Test]
    public void Unsubscribe_RemovesCorrectSubscription()
    {
        // Arrange
        var eventBus = new SignalBus();
        bool eventFired = false;
        Action<string> action = (message) => eventFired = true;

        // Act
        eventBus.Subscribe(action);
        eventBus.Fire("Test Event");
        eventBus.Unsubscribe(action);
        eventFired = false;
        eventBus.Fire("Test Event");

        // Assert
        Assert.IsFalse(eventFired);
    }

    [Test]
    public void Unsubscribe_WithNull_ThrowsException()
    {
        // Arrange
        var eventBus = new SignalBus();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => eventBus.Unsubscribe<string>(null));
    }

    [Test]
    public void Unsubscribe_ForNonExistentSubscription_DoesNot_CauseUnexpectedBehavior()
    {
        // Arrange
        var eventBus = new SignalBus();
        Action<string> action = (message) => { };

        // Act && Assert
        Assert.DoesNotThrow(() => eventBus.Unsubscribe(action));
    }


    [Test]
    public void Unsubscribe_RemovesOneInstance_Of_IdenticalAction()
    {
        // Arrange
        var eventBus = new SignalBus();
        int eventCount = 0;
        Action<int> action = (number) => eventCount++;

        // Act
        eventBus.Subscribe(action);
        eventBus.Subscribe(action);
        eventBus.Subscribe(action);
        eventBus.Unsubscribe(action);
        eventBus.Fire(5);

        // Assert
        Assert.AreEqual(2, eventCount);
    }
}
