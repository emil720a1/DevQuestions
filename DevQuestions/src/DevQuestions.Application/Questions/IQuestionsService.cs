using Contracts.Questions;

namespace DevQuestions.Application.Questions;

public interface IQuestionsService
{
    Task <Guid> Create(CreateQuestionDto request, CancellationToken cancellationToken);
}