using Shared;
using Shared.Exceptions;

namespace Questions.Application.Exceptions;

public class QuestionValidationException : BadRequestException
{
    public QuestionValidationException(Error[] errors) 
        : base(errors)
    {
    }
}