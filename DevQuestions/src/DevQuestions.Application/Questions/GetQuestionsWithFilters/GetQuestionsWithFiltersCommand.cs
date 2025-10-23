using Contracts.Questions;
using DevQuestions.Application.Abstractions;

namespace DevQuestions.Application.Questions.GetQuestionsWithFilters;

public record GetQuestionsWithFiltersCommand(GetQuestionsDto Dto) : ICommand;