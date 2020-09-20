using FluentValidation;
using System;
using System.Linq;
using TechTalkIntegrationTests.Domain.Models.Enums;

namespace TechTalkIntegrationTests.Domain.Models.Entities
{
    public class TaskDomain : BaseEntity
    {
        public TaskDomain(Guid id, string description, Priority priority, bool completed)
        {
            Id = id;
            Description = description;
            Priority = priority;
            Completed = completed;
            Activate();
        }

        public string Description { get; private set; }
        public Priority Priority { get; private set; }
        public bool Completed { get; set; }

        public void Update(string newDescription, bool newCompleted, Priority newPriority)
        {
            Description = newDescription;
            Completed = newCompleted;
            Priority = newPriority;
        }

        public void Complete()
        {
            Completed = true;
        }

        public override void Validate()
        {
            new TaskValidator().ValidateAndThrow(this);
        }
    }

    class TaskValidator : AbstractValidator<TaskDomain>
    {
        public TaskValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Cannot be empty!");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Cannot be empty!")
                .MaximumLength(200)
                .WithMessage("Cannot be longer than 200 characters!");

            var enumValues = string.Join("; ", Enum.GetValues(typeof(Priority)).Cast<int>());

            RuleFor(x => x.Priority)
                .IsInEnum()
                .WithMessage($"Must be one of the following values: {enumValues} !");
        }
    }
}
