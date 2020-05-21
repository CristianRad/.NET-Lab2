using FluentValidation;
using Lab2.Models;

namespace Lab2.ModelValidators
{
	public class CommentValidator : AbstractValidator<Comment>
	{
		public CommentValidator()
		{
			RuleFor(x => x.Text.Length).GreaterThan(10);
		}
	}
}
