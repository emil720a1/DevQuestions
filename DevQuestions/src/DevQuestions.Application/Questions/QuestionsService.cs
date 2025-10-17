using Contracts.Questions;
using DevQuestions.Application.Extensions;
using DevQuestions.Application.FulltextSearch;
using DevQuestions.Application.Questions.Exceptions;
using DevQuestions.Domain.Questions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared;

namespace DevQuestions.Application.Questions;

public class QuestionsService : IQuestionsService
{
    private readonly IQuestionsRepository _questionsRepository;
    private readonly ILogger<QuestionsService> _logger;
    private readonly IValidator<CreateQuestionDto> _validator;

    public QuestionsService(
        IQuestionsRepository questionsRepository, 
        IValidator<CreateQuestionDto> validator,
        ILogger<QuestionsService> logger)
        
    {
        _questionsRepository = questionsRepository;
        _validator = validator;
        _logger = logger;
    }
    public async Task<Guid> Create(CreateQuestionDto questionDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(questionDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new QuestionValidationException(validationResult.ToErrors());
        }
        
        var calculator = new QuestionCalculator();
        
        calculator.Calculate();
        
        int openUserQuestionsCount = await _questionsRepository
            .GetOpenUserQuestionsAsync(questionDto.UserId, cancellationToken);

        var existedQuestion = await _questionsRepository.GetByIdAsync(Guid.Empty, cancellationToken);

        if (openUserQuestionsCount > 3)
        {
            throw new ToManyQuestionsException();
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
    // public async Task<IActionResult> AddAnswer(
    //      Guid questionId,
    //      AddAnswerDto request, 
    //     CancellationToken cancellationToken)
    // {
    // }
}


public class QuestionCalculator
{
    public void Calculate()
    {
        throw new NotImplementedException();
    }
}