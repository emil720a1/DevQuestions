using Shared;
using Shared.Exceptions;

namespace Questions.Application.Exceptions;

public class QuestionsNotFoundException : NotFoundException
{
    protected QuestionsNotFoundException(Error[] errors) 
        : base(errors)
    {
    }
}