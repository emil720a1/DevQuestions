using Contracts.Questions;
using DevQuestions.Application.Abstractions;
using DevQuestions.Application.Questions;
using DevQuestions.Application.Questions.CreateQuestion;
using DevQuestions.Presenters.ResponseExtensions;
using Microsoft.AspNetCore.Mvc;

namespace DevQuestions.Presenters.Questions;

[ApiController]
[Route("[controller]")]
public class QuestionsController : ControllerBase
{
    
    
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] ICommandHandler<Guid, CreateQuestionCommand> handler,
        [FromBody] CreateQuestionDto request, 
        CancellationToken cancellationToken)
    {
        
        var command = new CreateQuestionCommand(request);
        
        var result = await handler.Handle(command, cancellationToken);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetQuestionsDto request, CancellationToken cancellationToken)
    {
        return Ok("Questions get");
    }
    
    [HttpGet("{questionId:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid questionId, CancellationToken cancellationToken)
    {
        return Ok("Questions get");
    }
    
    [HttpPut("{questionId:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid questionId, 
        [FromBody] UpdateQuestionDto request, 
        CancellationToken cancellationToken)
    {
        return Ok("Questions updated");
    }
    
    
    [HttpDelete("{questionId:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid questionId, CancellationToken cancellationToken)
    {
        return Ok("Question deleted");
    }

    [HttpPut("{questionId:guid}/correct_solution")]
    public async Task<IActionResult> SelectSolution(
        [FromRoute] Guid questionsId, 
        [FromQuery] Guid answerId, 
        CancellationToken cancellationToken)
    {
        return Ok("Solutions selected");
    }
    
    [HttpPost("{questionId:guid}/answers")]
    public async Task<IActionResult> AddAnswer(
        [FromServices] ICommandHandler<Guid, AddAnswerCommand> handler,
        [FromRoute] Guid questionId,
        [FromBody] AddAnswerDto request, 
        CancellationToken cancellationToken)
    {
        
        var command = new AddAnswerCommand(questionId , request);
        
        var result = await handler.Handle(command, cancellationToken);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
        
    }
}