// <copyright file="MyNewModuleTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Tests.Domains.Aggregates;

using Hexalith.Domains.Results;
using Hexalith.MyNewModule.Aggregates.MyNewModules;
using Hexalith.MyNewModule.Events;
using Hexalith.MyNewModule.Events.MyNewModules;

using Shouldly;

using Xunit;

/// <summary>
/// Tests for the MyNewModule aggregate.
/// </summary>
public class MyNewModuleTests
{
    /// <summary>
    /// Test that applying a MyNewModuleAdded event to an uninitialized aggregate succeeds.
    /// </summary>
    [Fact]
    public void ShouldApplyAddedEventToUninitializedAggregate()
    {
        // Arrange
        var aggregate = new MyNewModule();
        var addedEvent = new MyNewModuleAdded("test-id", "Test Module", "Test Description", 1.5m);

        // Act
        ApplyResult result = aggregate.Apply(addedEvent);

        // Assert
        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<ApplyResult>();
    }

    /// <summary>
    /// Test that a new MyNewModule can be created from a MyNewModuleAdded event.
    /// </summary>
    [Fact]
    public void ShouldCreateMyNewModuleFromAddedEvent()
    {
        // Arrange
        var addedEvent = new MyNewModuleAdded("test-id", "Test Module", "Test Description", 1.5m);

        // Act
        var myNewModule = new MyNewModule(addedEvent);

        // Assert
        myNewModule.Id.ShouldBe("test-id");
        myNewModule.Name.ShouldBe("Test Module");
        myNewModule.Comments.ShouldBe("Test Description");
        myNewModule.PriorityWeight.ShouldBe(1.5m);
        myNewModule.Disabled.ShouldBeFalse();
    }

    /// <summary>
    /// Test that applying a MyNewModuleDisabled event disables the module.
    /// </summary>
    [Fact]
    public void ShouldDisableModuleWhenApplyingDisabledEvent()
    {
        // Arrange
        var aggregate = new MyNewModule("test-id", "Test Module", "Test Description", 1.5m, false);
        var disabledEvent = new MyNewModuleDisabled("test-id");

        // Act
        ApplyResult result = aggregate.Apply(disabledEvent);

        // Assert
        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<ApplyResult>();
        _ = result.Aggregate.ShouldBeOfType<MyNewModule>();
        ((MyNewModule)result.Aggregate).Disabled.ShouldBeTrue();
    }

    /// <summary>
    /// Test that applying a MyNewModuleEnabled event enables a disabled module.
    /// </summary>
    [Fact]
    public void ShouldEnableDisabledModuleWhenApplyingEnabledEvent()
    {
        // Arrange
        var aggregate = new MyNewModule("test-id", "Test Module", "Test Description", 1.5m, true);
        var enabledEvent = new MyNewModuleEnabled("test-id");

        // Act
        ApplyResult result = aggregate.Apply(enabledEvent);

        // Assert
        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<ApplyResult>();
        _ = result.Aggregate.ShouldBeOfType<MyNewModule>();
        ((MyNewModule)result.Aggregate).Disabled.ShouldBeFalse();
    }

    /// <summary>
    /// Test that applying a MyNewModuleAdded event to an already initialized aggregate fails.
    /// </summary>
    [Fact]
    public void ShouldFailWhenAddingToAlreadyInitializedAggregate()
    {
        // Arrange
        var aggregate = new MyNewModule("test-id", "Test Module", "Test Description", 1.5m, false);
        var addedEvent = new MyNewModuleAdded("test-id", "Another Module", "Another Description", 2.0m);

        // Act
        ApplyResult result = aggregate.Apply(addedEvent);

        // Assert
        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<ApplyResult>();
    }

    /// <summary>
    /// Test that applying a MyNewModuleDisabled event to an already disabled module returns an error.
    /// </summary>
    [Fact]
    public void ShouldFailWhenDisablingAlreadyDisabledModule()
    {
        // Arrange
        var aggregate = new MyNewModule("test-id", "Test Module", "Test Description", 1.5m, true);
        var disabledEvent = new MyNewModuleDisabled("test-id");

        // Act
        ApplyResult result = aggregate.Apply(disabledEvent);

        // Assert
        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<ApplyResult>();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Test that applying a MyNewModuleEnabled event to an already enabled module returns an error.
    /// </summary>
    [Fact]
    public void ShouldFailWhenEnablingAlreadyEnabledModule()
    {
        // Arrange
        var aggregate = new MyNewModule("test-id", "Test Module", "Test Description", 1.5m, false);
        var enabledEvent = new MyNewModuleEnabled("test-id");

        // Act
        ApplyResult result = aggregate.Apply(enabledEvent);

        // Assert
        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<ApplyResult>();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Test that applying a MyNewModuleDescriptionChanged event with the same values returns an error.
    /// </summary>
    [Fact]
    public void ShouldFailWhenSettingSameDescription()
    {
        // Arrange
        var aggregate = new MyNewModule("test-id", "Test Module", "Test Description", 1.5m, false);
        var descriptionChangedEvent = new MyNewModuleDescriptionChanged("test-id", "Test Module", "Test Description");

        // Act
        ApplyResult result = aggregate.Apply(descriptionChangedEvent);

        // Assert
        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<ApplyResult>();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Test that applying a MyNewModulePriorityWeightChanged event with the same value returns an error.
    /// </summary>
    [Fact]
    public void ShouldFailWhenSettingSamePriorityWeight()
    {
        // Arrange
        var aggregate = new MyNewModule("test-id", "Test Module", "Test Description", 1.5m, false);
        var priorityWeightChangedEvent = new MyNewModulePriorityWeightChanged("test-id", 1.5m);

        // Act
        ApplyResult result = aggregate.Apply(priorityWeightChangedEvent);

        // Assert
        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<ApplyResult>();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Test that AggregateId property returns the correct Id.
    /// </summary>
    [Fact]
    public void ShouldReturnCorrectAggregateId()
    {
        // Arrange
        var aggregate = new MyNewModule("test-id", "Test Module", "Test Description", 1.5m, false);

        // Act & Assert
        aggregate.AggregateId.ShouldBe("test-id");
    }

    /// <summary>
    /// Test that AggregateName property returns the correct aggregate name.
    /// </summary>
    [Fact]
    public void ShouldReturnCorrectAggregateName()
    {
        // Arrange
        var aggregate = new MyNewModule("test-id", "Test Module", "Test Description", 1.5m, false);

        // Act & Assert
        aggregate.AggregateName.ShouldBe(MyNewModuleDomainHelper.MyNewModuleAggregateName);
    }

    /// <summary>
    /// Test that applying invalid (non-MyNewModule) events returns InvalidEvent result.
    /// </summary>
    [Fact]
    public void ShouldReturnInvalidEventWhenApplyingNonMyNewModuleEvent()
    {
        // Arrange
        var aggregate = new MyNewModule("test-id", "Test Module", "Test Description", 1.5m, false);
        object invalidEvent = new();

        // Act
        ApplyResult result = aggregate.Apply(invalidEvent);

        // Assert
        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<ApplyResult>();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Test that applying events to a disabled module (except enable/disable) returns NotEnabled result.
    /// </summary>
    [Fact]
    public void ShouldReturnNotEnabledWhenApplyingEventsToDisabledModule()
    {
        // Arrange
        var aggregate = new MyNewModule("test-id", "Test Module", "Test Description", 1.5m, true);
        var descriptionChangedEvent = new MyNewModuleDescriptionChanged("test-id", "New Name", "New Description");

        // Act
        ApplyResult result = aggregate.Apply(descriptionChangedEvent);

        // Assert
        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<ApplyResult>();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Test that applying non-Added events to uninitialized aggregate returns NotInitialized result.
    /// </summary>
    [Fact]
    public void ShouldReturnNotInitializedWhenApplyingEventsToUninitializedAggregate()
    {
        // Arrange
        var aggregate = new MyNewModule();
        var descriptionChangedEvent = new MyNewModuleDescriptionChanged("test-id", "New Name", "New Description");

        // Act
        ApplyResult result = aggregate.Apply(descriptionChangedEvent);

        // Assert
        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<ApplyResult>();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Test that Apply method throws ArgumentNullException when called with null event.
    /// </summary>
    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenApplyCalledWithNull()
    {
        // Arrange
        var aggregate = new MyNewModule("test-id", "Test Module", "Test Description", 1.5m, false);

        // Act & Assert
        _ = Should.Throw<ArgumentNullException>(() => aggregate.Apply(null!));
    }

    /// <summary>
    /// Test that constructor with null MyNewModuleAdded event throws ArgumentNullException.
    /// </summary>
    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenConstructorCalledWithNullEvent() =>

        // Arrange & Act & Assert
        Should.Throw<ArgumentNullException>(() => new MyNewModule(null!));

    /// <summary>
    /// Test that applying a MyNewModuleDescriptionChanged event updates the name and comments.
    /// </summary>
    [Fact]
    public void ShouldUpdateDescriptionWhenApplyingDescriptionChangedEvent()
    {
        // Arrange
        var aggregate = new MyNewModule("test-id", "Old Name", "Old Description", 1.5m, false);
        var descriptionChangedEvent = new MyNewModuleDescriptionChanged("test-id", "New Name", "New Description");

        // Act
        ApplyResult result = aggregate.Apply(descriptionChangedEvent);

        // Assert
        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<ApplyResult>();
        _ = result.Aggregate.ShouldBeOfType<MyNewModule>();
        var updatedModule = (MyNewModule)result.Aggregate;
        updatedModule.Name.ShouldBe("New Name");
        updatedModule.Comments.ShouldBe("New Description");
    }

    /// <summary>
    /// Test that applying a MyNewModulePriorityWeightChanged event updates the priority weight.
    /// </summary>
    [Fact]
    public void ShouldUpdatePriorityWeightWhenApplyingPriorityWeightChangedEvent()
    {
        // Arrange
        var aggregate = new MyNewModule("test-id", "Test Module", "Test Description", 1.5m, false);
        var priorityWeightChangedEvent = new MyNewModulePriorityWeightChanged("test-id", 2.5m);

        // Act
        ApplyResult result = aggregate.Apply(priorityWeightChangedEvent);

        // Assert
        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<ApplyResult>();
        _ = result.Aggregate.ShouldBeOfType<MyNewModule>();
        ((MyNewModule)result.Aggregate).PriorityWeight.ShouldBe(2.5m);
    }
}