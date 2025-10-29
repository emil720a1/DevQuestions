using CSharpFunctionalExtensions;
using Questions.Domain;

namespace Shared.FulltextSearch;

public  interface ISearchProvider
{
    Task<List<Guid>> SearchAsync(string query);
    
    Task<UnitResult<Failure>> IndexQuestionAsync(Question question);
}