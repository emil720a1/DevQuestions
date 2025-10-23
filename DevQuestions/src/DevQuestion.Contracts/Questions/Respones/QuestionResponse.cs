using Contracts.Questions.Dtos;

namespace Contracts.Questions.Respones;

public record QuestionResponse(IEnumerable<QuestionDto> Questions , long TotalCount);