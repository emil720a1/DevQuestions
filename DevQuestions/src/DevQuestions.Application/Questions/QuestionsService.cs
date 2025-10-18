using Contracts.Questions;
using CSharpFunctionalExtensions;
using DevQuestions.Application.Extensions;
using DevQuestions.Domain.Questions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared;

namespace DevQuestions.Application.Questions;

public class QuestionsService : IQuestionsService
{
    private readonly IQuestionsRepository _questionsRepository;
    private readonly ILogger<QuestionsService> _logger;
    private readonly IValidator<CreateQuestionDto> _createQuestionDtoValidator;
    private readonly IValidator<AddAnswerDto> _addAnswerDtoValidator;

    public QuestionsService(
        IQuestionsRepository questionsRepository, 
        IValidator<CreateQuestionDto> createQuestionDtoValidator,
        IValidator<AddAnswerDto> addAnswerDtoValidator,
        ILogger<QuestionsService> logger)
    
        
    {
        _questionsRepository = questionsRepository;
        _addAnswerDtoValidator = addAnswerDtoValidator;
        _createQuestionDtoValidator = createQuestionDtoValidator;
        _logger = logger;
    }

    public async Task<Result<Guid, Failure>> Create(CreateQuestionDto questionDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createQuestionDtoValidator.ValidateAsync(questionDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToErrors();
        }
        
        int openUserQuestionsCount = await _questionsRepository
            .GetOpenUserQuestionsAsync(questionDto.UserId, cancellationToken);

        var existedQuestion = await _questionsRepository.GetByIdAsync(Guid.Empty, cancellationToken);

        if (openUserQuestionsCount > 3)
        {
            return Errors.Questions.ToManyQuestions().ToFailure();
        } 

        var questionId = Guid.NewGuid();
        
        var question = new Question(
            questionId,
            questionDto.Title,
            questionDto.Text,
            questionDto.UserId,
            null,
            questionDto.TagIds);
        
        

        await _questionsRepository.AddAsync(question, cancellationToken);

        _logger.LogInformation("Question created with id {QuestionId}", questionId);
        
        return questionId;
    }
    
    
    // public async Task<IActionResult> Update(
    //      Guid questionId, 
    //      UpdateQuestionDto request, 
    //     CancellationToken cancellationToken)
    // {
    // }
    //
    //
    // public async Task<IActionResult> Delete( Guid questionId, CancellationToken cancellationToken)
    // {
    // }
    //
    // public async Task<IActionResult> SelectSolution(
    //      Guid questionsId, 
    //      Guid answerId, 
    //     CancellationToken cancellationToken)
    // {
    // }
    //
    public async Task<Result<Guid, Failure>> AddAnswer(
         Guid questionId,
         AddAnswerDto addAnswerDto, 
        CancellationToken cancellationToken)
    {
        var validationResult = await _addAnswerDtoValidator.ValidateAsync(addAnswerDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToErrors();
        }
        
        var questionResult = await _questionsRepository.GetByIdAsync(questionId, cancellationToken);
        if (questionResult.IsFailure)
        {
            return questionResult.Error;
        }
        
        var answer = new Answer(Guid.NewGuid(), addAnswerDto.UserId, addAnswerDto.Text, questionId);
        
        var answerId = await _questionsRepository.AddAnswerAsync(answer, cancellationToken);
        
        _logger.LogInformation("Answer added with id {AnswerId} to question {questionId}", answerId, questionId);
        
        return answerId;
            
    }
    
}


