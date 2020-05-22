using FluentValidation;
using Lab2.Models;
using System;

namespace Lab2.ModelValidators
{
    public class TaskValidator : AbstractValidator<Task>
    {
        public TaskValidator()
        {
            RuleFor(t => t.Added).LessThan(t => t.Deadline);
            RuleFor(t => t.Added).GreaterThanOrEqualTo(DateTime.Now);
            RuleFor(t => t.Title).MaximumLength(20);
            RuleFor(t => t.Description).MinimumLength(10);
        }
    }
}
