using Contracts.Questions.Dtos;
using Contracts.Questions.Respones;
using CSharpFunctionalExtensions;
using DevQuestions.Application.Abstractions;
using DevQuestions.Application.FilesStorage;
using DevQuestions.Application.Tags;
using DevQuestions.Domain.Questions;
using Shared;

namespace DevQuestions.Application.Questions.GetQuestionsWithFilters;

public class GetQuestionsWithFilter : IHandler<QuestionResponse, GetQuestionsWithFiltersCommand>
{
    
    private readonly IQuestionsRepository _questionsRepository;
    private readonly IFilesProvider _filesProvider;
    private readonly ITagsRepository _tagsRepository;
    

    public GetQuestionsWithFilter(
        IQuestionsRepository questionsRepository, 
        IFilesProvider filesProvider, 
        ITagsRepository tagsRepository)
    {
        _questionsRepository = questionsRepository;
        _filesProvider = filesProvider;
        _tagsRepository = tagsRepository;
    }

    public async Task<Result<QuestionResponse, Failure>> Handle(GetQuestionsWithFiltersCommand command, CancellationToken cancellationToken)
    {
        (IReadOnlyList<Question> questions,long count) = await _questionsRepository.GetQuestionsWithFilterAsync(command, cancellationToken);

        var screenshotIds = questions
            .Where(q => q.ScreenshotId is not null)
            .Select(q => q.ScreenshotId!.Value);
        
        var filesDict = await _filesProvider.GetUrlsByIdsAsync(screenshotIds, cancellationToken);
        
        var questionTags = questions.SelectMany(q => q.Tags);

        var tags = await _tagsRepository.GetTagsAsync(questionTags, cancellationToken);
        

        var questionsDto = questions.Select(q => new QuestionDto(
            q.Id,
            q.Title,
            q.Text, 
            q.UserId,
            q.ScreenshotId is not null ? filesDict[q.ScreenshotId!.Value] : null,
            q.Solution?.Id,
            tags,
            q.Status.ToStringUa()
            ));
        
        return new QuestionResponse(questionsDto, count);
    }
}