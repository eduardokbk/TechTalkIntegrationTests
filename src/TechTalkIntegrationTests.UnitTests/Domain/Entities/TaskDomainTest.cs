using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalkIntegrationTests.Domain.Models.Entities;
using TechTalkIntegrationTests.Domain.Models.Enums;
using Xunit;

namespace TechTalkIntegrationTests.UnitTests.Domain.Entities
{
    public class TarefaDomainTest
    {

        [Fact]
        public void Constructor_ShouldCreateAnActiveTask()
        {
            // Arrange
            var id = Guid.NewGuid();
            var description = "test";
            var completed = false;
            var priority = Priority.High;

            // Act
            var taskDomain = new TaskDomain(id, description, priority, completed);

            // Assert
            taskDomain.Id.Should().Be(id);
            taskDomain.Description.Should().Be(description);
            taskDomain.Priority.Should().Be(priority);
            taskDomain.Completed.Should().Be(completed);
            taskDomain.Active.Should().BeTrue();
        }

        [Fact]
        public void Update_ShouldUpdateValues()
        {
            // Arrange
            var id = Guid.NewGuid();
            var taskDomain = new TaskDomain(id, "old description", 0, false);

            var newDescription = "test";
            var newCompleted = true;
            var newPriority = Priority.High;

            // Act
            taskDomain.Update(newDescription, newCompleted, newPriority);

            // Assert
            taskDomain.Id.Should().Be(id);
            taskDomain.Description.Should().Be(newDescription);
            taskDomain.Priority.Should().Be(newPriority);
            taskDomain.Completed.Should().Be(newCompleted);
            taskDomain.Active.Should().BeTrue();
        }

        [Fact]
        public void Complete_ShouldCompleteTask()
        {
            // Arrange
            var taskDomain = new TaskDomain(Guid.NewGuid(), "", 0, false);
            // Act
            taskDomain.Complete();

            // Assert
            taskDomain.Completed.Should().BeTrue();
        }

        [Fact]
        public void Complete_WithValidValues_ShouldNotThrowException()
        {
            // Arrange
            var taskDomain = new TaskDomain(Guid.NewGuid(), "test", Priority.High, false);
            // Act
            Action act = () => taskDomain.Validate();

            // Assert
            act.Should().NotThrow<Exception>();
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void Complete_WithInvalidValues_ShouldThrowException(TaskDomain taskDomain, string message)
        {
            // Arrange && Act
            Action act = () => taskDomain.Validate();

            // Assert
            act.Should().Throw<Exception>().WithMessage($"*{message}*");
        }

        public static IEnumerable<object[]> GetData()
        {
            var enumValues = string.Join("; ", Enum.GetValues(typeof(Priority)).Cast<int>());

            yield return new object[]
            {
                new TaskDomain(Guid.Empty, "test", Priority.High, true),
                "Id: Cannot be empty!"
            };

            yield return new object[]
            {
                new TaskDomain(Guid.NewGuid(), "", Priority.High, true),
                "Description: Cannot be empty!"
            };

            yield return new object[]
            {
                new TaskDomain(Guid.NewGuid(), new string('a', 201), Priority.High, true),
                "Description: Cannot be longer than 200 characters!"
            };

            yield return new object[]
            {
                new TaskDomain(Guid.NewGuid(), new string('a', 200), (Priority)(-1), true),
                $"Priority: Must be one of the following values: {enumValues} !"
            };
        }
    }
}