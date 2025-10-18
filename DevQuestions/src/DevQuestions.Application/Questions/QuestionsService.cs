using Contracts.Questions;
using CSharpFunctionalExtensions;
using DevQuestions.Application.Communication;
using DevQuestions.Application.Database;
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
    private readonly ITransactionManager _transactionManager;
    private readonly IValidator<SelectSolutionDto> _selectSolutionDtoValidator;
    private readonly IUsersCommunicationService _usersCommunicationService;

    public QuestionsService(
        IQuestionsRepository questionsRepository, 
        IValidator<CreateQuestionDto> createQuestionDtoValidator,
        IValidator<AddAnswerDto> addAnswerDtoValidator,
        ITransactionManager transactionManager,
        IUsersCommunicationService usersCommunicationService,
        IValidator<SelectSolutionDto> selectSolutionDtoValidator,
        ILogger<QuestionsService> logger)
    
        
    {
        _questionsRepository = questionsRepository;
        _addAnswerDtoValidator = addAnswerDtoValidator;
        _createQuestionDtoValidator = createQuestionDtoValidator;
        _transactionManager = transactionManager;
        _usersCommunicationService = usersCommunicationService;
        _selectSolutionDtoValidator = selectSolutionDtoValidator;
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
    public async Task<Result<Guid, Failure>> SelectSolution(
        Guid questionsId,
        Guid answerId,
        SelectSolutionDto selectSolutionDto,
        CancellationToken cancellationToken)
    {

        var validationResult = await _selectSolutionDtoValidator.ValidateAsync(selectSolutionDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToErrors();
        }


        var transaction = await _transactionManager.BeginTransactionAsync(cancellationToken);

        (_, bool isFailure, Question? question, Failure? error) =
            await _questionsRepository.GetByIdAsync(questionsId, cancellationToken);
        if (isFailure)
        {
            return error;
        }
        
       var selectedAnswer =  question.Solution = question.Answers.FirstOrDefault(a => a.Id == answerId);
        question.Status = QuestionStatus.RESOLVED;

        await _questionsRepository.SaveAsync(question, cancellationToken);

        transaction.Commit();
        
        _logger.LogInformation("Answer added with id {AnswerId} to question {questionId}", answerId, questionsId);
        
        
        return answerId;
    }

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
        
        var usersRatingResult = await _usersCommunicationService.GetUserRatingAsync(addAnswerDto.UserId, cancellationToken);
        if (usersRatingResult.IsFailure)
        {
            return usersRatingResult.Error;
        }
        
        if (usersRatingResult.Value <= 0)
        {
            _logger.LogError("User with id {userId} has no rating", addAnswerDto.UserId);
            return Errors.Questions.NotEnoughRating();
        }
        
        var transaction = await _transactionManager.BeginTransactionAsync (cancellationToken);
        
        (_, bool isFailure, Question? question, Failure? error) = await _questionsRepository.GetByIdAsync(questionId, cancellationToken);
        if (isFailure)
        {
            return error;
        }

        var answer = new Answer(Guid.NewGuid(), addAnswerDto.UserId, addAnswerDto.Text, questionId);
        
        question.Answers.Add(answer);
        
         await _questionsRepository.SaveAsync(question, cancellationToken);
        
        transaction.Commit();
        
        _logger.LogInformation("Answer added with id {AnswerId} to question {questionId}", answer.Id, questionId);
        
        return answer.Id;
            
    }
    
}


