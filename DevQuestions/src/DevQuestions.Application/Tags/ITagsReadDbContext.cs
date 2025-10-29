using Amazon.S3.Model;


namespace DevQuestions.Application.Tags;

public interface ITagsReadDbContext
{
    IQueryable<Tag> TagsRead { get; }
}