// using Contracts.Questions;

using Questions.Contracts.Dtos;
using Shared.Abstractions;

namespace Questions.Application.Features;

public record AddAnswerCommand(Guid QuestionId, AddAnswerDto AddAnswerDto) : ICommand;