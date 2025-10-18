using CSharpFunctionalExtensions;
using DevQuestions.Application.FulltextSearch;
using DevQuestions.Domain.Questions;
using Shared;

namespace DevQuestion.Infrastructure.ElasticSearch;

public class ElasticSearchProvider : ISearchProvider
{
     
    public Task<List<Guid>> SearchAsync(string query) => throw new NotImplementedException();

    public async Task<UnitResult<Failure>> IndexQuestionAsync(Question question)
    {
        try
        {

        }
        catch (Exception e)
        {
            return Error.Failure("search.error", e.Message).ToFailure();
        }

        return UnitResult.Success<Failure>();
    }
}