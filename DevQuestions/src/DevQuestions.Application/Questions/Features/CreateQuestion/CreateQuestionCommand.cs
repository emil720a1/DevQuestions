using Contracts.Questions;
using DevQuestions.Application.Abstractions;

namespace DevQuestions.Application.Questions.Features.CreateQuestion;

public record CreateQuestionCommand(CreateQuestionDto QuestionDto) : ICommand;