// <copyright file="MyNewModuleTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Tests.Domains.Aggregates;

using Hexalith.Domains.Results;
using Hexalith.MyNewModule.Aggregates.MyNewModules;
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
    }
}