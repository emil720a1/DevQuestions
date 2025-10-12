namespace Contracts.Questions;

public record UpdateQuestionDto(string Title, string Body, Guid[] TagIds);