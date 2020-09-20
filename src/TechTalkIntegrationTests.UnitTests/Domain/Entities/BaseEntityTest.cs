using FluentAssertions;
using TechTalkIntegrationTests.Domain.Models.Entities;
using Xunit;

namespace TechTalkIntegrationTests.UnitTests.Domain.Entities
{
    public class BaseEntityTest
    {
        [Fact]
        public void Activate_ShouldSetActiveToTrue()
        {
            // Arrange
            var entity = new TestEntity(false);

            // Act
            entity.Activate();

            // Assert
            entity.Active.Should().BeTrue();
        }

        [Fact]
        public void Inactivate_ShouldSetActiveToTrue()
        {
            // Arrange
            var entity = new TestEntity(true);

            // Act
            entity.Inactivate();

            // Assert
            entity.Active.Should().BeFalse();
        }
    }

    class TestEntity : BaseEntity
    {
        public TestEntity(bool active)
        {
            Active = active;
        }

        public override void Validate(){}
    }
}
