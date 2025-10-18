using Contracts.Questions;
using DevQuestions.Application.Abstractions;

namespace DevQuestions.Application.Questions.CreateQuestion;

public record CreateQuestionCommand(CreateQuestionDto QuestionDto) : ICommand;