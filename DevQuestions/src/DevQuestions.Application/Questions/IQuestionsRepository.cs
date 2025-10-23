using CSharpFunctionalExtensions;
using DevQuestions.Application.Questions.GetQuestionsWithFilters;
using DevQuestions.Domain.Questions;
using Shared;

namespace DevQuestions.Application.Questions;

public interface IQuestionsRepository
{
    Task<Guid> AddAsync(Question question, CancellationToken cancellationToken);

    Task<Guid> SaveAsync(Question question, CancellationToken cancellationToken);
    
    Task<Guid> DeleteAsync(Guid questionId, CancellationToken cancellationToken);
    
    Task<Result<Question, Failure>> GetByIdAsync(Guid questionId, CancellationToken cancellationToken);

    Task<(IReadOnlyList<Question> Questions, long Count)> GetQuestionsWithFilterAsync(GetQuestionsWithFiltersCommand command, CancellationToken cancellationToken);
    
    Task<int> GetOpenUserQuestionsAsync(Guid userId, CancellationToken cancellationToken);
}