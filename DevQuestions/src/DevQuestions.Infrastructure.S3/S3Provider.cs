using DevQuestions.Application.FilesStorage;

namespace DevQuestions.Infrastructure.S3;

public class S3Provider : IFilesProvider
{
    private IFilesProvider _filesProviderImplementation;

    public Task<string> UploadAsync(Stream stream, string key, string bucket) => throw new NotImplementedException();
    public Task<string> GetUrlByIdAsync(Guid fileId, CancellationToken cancellationToken) => _filesProviderImplementation.GetUrlByIdAsync(fileId, cancellationToken);

    public Task<Dictionary<Guid, string>> GetUrlsByIdsAsync(IEnumerable<Guid> fileIds, CancellationToken cancellationToken) => _filesProviderImplementation.GetUrlsByIdsAsync(fileIds, cancellationToken);
}