﻿using Contracts.Questions;
using CSharpFunctionalExtensions;
using DevQuestions.Application.Abstractions;
using DevQuestions.Application.Communication;
using DevQuestions.Application.Database;
using DevQuestions.Application.Extensions;
using DevQuestions.Domain.Questions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared;

namespace DevQuestions.Application.Questions;

public class AddAnswerHandler : ICommandHandler<Guid, AddAnswerCommand>
{

    private readonly IQuestionsRepository _questionsRepository;
    private readonly ILogger<AddAnswerHandler> _logger;
    private readonly IValidator<AddAnswerDto> _validator;
    private readonly ITransactionManager _transactionManager;
    private readonly IUsersCommunicationService _usersCommunicationService;
    public AddAnswerHandler(
        IQuestionsRepository questionsRepository, 
        ILogger<AddAnswerHandler> logger,
        IValidator<AddAnswerDto> validator,
        ITransactionManager transactionManager, 
        IUsersCommunicationService usersCommunicationService)
    {
        _questionsRepository = questionsRepository;
        _validator = validator;
        _transactionManager = transactionManager;
        _usersCommunicationService = usersCommunicationService;
        _logger = logger;
    }
    public async Task<Result<Guid, Failure>> Handle(
        AddAnswerCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command.AddAnswerDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToErrors();
        }
        
        var usersRatingResult = await _usersCommunicationService
            .GetUserRatingAsync(command.AddAnswerDto.UserId, cancellationToken);
        
        if (usersRatingResult.IsFailure)
        {
            return usersRatingResult.Error;
        }
        
        if (usersRatingResult.Value <= 0)
        {
            _logger.LogError("User with id {userId} has no rating", command.AddAnswerDto.UserId);
            return Errors.Questions.NotEnoughRating();
        }
        
        var transaction = await _transactionManager.BeginTransactionAsync (cancellationToken);
        
        (_, bool isFailure, Question? question, Failure? error) = await _questionsRepository.GetByIdAsync(command.QuestionId, cancellationToken);
        if (isFailure)
        {
            return error;
        }

        var answer = new Answer(Guid.NewGuid(), command.AddAnswerDto.UserId, command.AddAnswerDto.Text, command.QuestionId);
        
        question.Answers.Add(answer);
        
        await _questionsRepository.SaveAsync(question, cancellationToken);
        
        transaction.Commit();
        
        _logger.LogInformation("Answer added with id {AnswerId} to question {questionId}", answer.Id, command.QuestionId);
        
        return answer.Id;
            
    }
}