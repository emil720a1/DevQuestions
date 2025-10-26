using Contracts.Questions;
using Contracts.Questions.Dtos;
using DevQuestions.Application.Abstractions;

namespace DevQuestions.Application.Questions.Features;

public record AddAnswerCommand(Guid QuestionId, AddAnswerDto AddAnswerDto) : ICommand;