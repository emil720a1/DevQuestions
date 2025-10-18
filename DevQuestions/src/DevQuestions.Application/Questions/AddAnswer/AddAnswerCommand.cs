using Contracts.Questions;
using DevQuestions.Application.Abstractions;

namespace DevQuestions.Application.Questions;

public record AddAnswerCommand(Guid QuestionId, AddAnswerDto AddAnswerDto) : ICommand;