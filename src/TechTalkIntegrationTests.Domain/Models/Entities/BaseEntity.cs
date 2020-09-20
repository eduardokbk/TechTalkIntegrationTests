using System;

namespace TechTalkIntegrationTests.Domain.Models.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }
        public bool Active { get; protected set; }

        public void Activate()
        {
            Active = true;
        }

        public void Inactivate()
        {
            Active = false;
        }

        public abstract void Validate();
    }
}
