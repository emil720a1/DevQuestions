namespace DevQuestions.Application.Questions.Features.SelectSolution;
//
// public class SelectSolutionHandler : ICommandHandler<Guid, SelectSolutionCommand>
// {
//     private readonly IQuestionsRepository _questionsRepository;
//     private readonly ITransactionManager _transactionManager;
//     private readonly IValidator<SelectSolutionDto> _selectSolutionDtoValidator;
//     private readonly ILogger<QuestionsService> _logger;
//     
//
//     public SelectSolutionHandler(
//         IQuestionsRepository questionsRepository, 
//         ITransactionManager transactionManager, 
//         IValidator<SelectSolutionDto> selectSolutionDtoValidator, 
//         ILogger<QuestionsService> logger)
//     {
//         _questionsRepository = questionsRepository;
//         _transactionManager = transactionManager;
//         _selectSolutionDtoValidator = selectSolutionDtoValidator;
//         _logger = logger;
//     }
//
//     public async Task<Result<Guid, Failure>> Handle(
//         Guid questionsId,
//         Guid answerId,
//         SelectSolutionCommand command,
//         CancellationToken cancellationToken)
//     {
//
//         var validationResult = await _selectSolutionDtoValidator.ValidateAsync(selectSolutionDto, cancellationToken);
//         if (!validationResult.IsValid)
//         {
//             return validationResult.ToErrors();
//         }
//
//
//         var transaction = await _transactionManager.BeginTransactionAsync(cancellationToken);
//
//         (_, bool isFailure, Question? question, Failure? error) =
//             await _questionsRepository.GetByIdAsync(questionsId, cancellationToken);
//         if (isFailure)
//         {
//             return error;
//         }
//         
//         var selectedAnswer =  question.Solution = question.Answers.FirstOrDefault(a => a.Id == answerId);
//         question.Status = QuestionStatus.RESOLVED;
//
//         await _questionsRepository.SaveAsync(question, cancellationToken);
//
//         transaction.Commit();
//         
//         _logger.LogInformation("Answer added with id {AnswerId} to question {questionId}", answerId, questionsId);
//         
//         
//         return answerId;
//     }
// }