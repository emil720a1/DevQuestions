using Contracts.Questions;
using Contracts.Questions.Dtos;
using CSharpFunctionalExtensions;
using Shared;

namespace DevQuestions.Application.Questions;

public interface IQuestionsService
{
    
    /// <summary>
    /// Создание вопроса
    /// </summary>
    /// <param name="questionDto">DTO для создания вопроса.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат работы метода. Либо ID созданого вопроса, либо список ошибок.</returns>
    Task<Result<Guid, Failure>> Create(CreateQuestionDto request, CancellationToken cancellationToken);

    
    /// <summary>
    /// Добавление ответа на вопрос.
    /// </summary>
    /// <param name="questionId">ID вопроса.</param>
    /// <param name="addAnswerDto">DTO для добавления ответа на вопрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат роботы метода - либо ID созданого ответа, либо список ошибок.</returns>
    Task<Result<Guid, Failure>> AddAnswer(Guid questionId, AddAnswerDto addAnswerDto, CancellationToken cancellationToken);


}

