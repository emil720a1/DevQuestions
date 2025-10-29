using Questions.Contracts.Dtos;

namespace Questions.Contracts.Respones;

public record QuestionResponse(IEnumerable<QuestionDto> Questions , long TotalCount);