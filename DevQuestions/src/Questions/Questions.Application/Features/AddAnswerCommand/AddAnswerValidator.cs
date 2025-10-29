// using Contracts.Questions;

using FluentValidation;
using Questions.Contracts.Dtos;

namespace Questions.Application.Features;

public class AddAnswerValidator : AbstractValidator<AddAnswerDto>
{
    public AddAnswerValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Текст не может быть пустым.")
            .MaximumLength(5000).WithMessage("Текст невалидный.");
    }
}