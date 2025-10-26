using Contracts.Questions.Dtos;
using DevQuestions.Application.Abstractions;

namespace DevQuestions.Application.Questions.GetQuestionsWithFiltersQuery;

public record GetQuestionsWithFiltersQuery(GetQuestionsDto Dto) : IQuery;