using CSharpFunctionalExtensions;
using Questions.Domain;
using Shared;
using Shared.FulltextSearch;

namespace Infrastructure.ElasticSearch;

public class ElasticSearchProvider : ISearchProvider
{
    private ISearchProvider _searchProviderImplementation;


    public Task<List<Guid>> SearchAsync(string query) => _searchProviderImplementation.SearchAsync(query);
    public Task<UnitResult<Failure>> IndexQuestionAsync(Question question) => throw new NotImplementedException();

}