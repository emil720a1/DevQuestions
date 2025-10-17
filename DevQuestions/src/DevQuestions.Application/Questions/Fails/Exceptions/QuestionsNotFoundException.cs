using DevQuestions.Application.Exceptions;
using Shared;

namespace DevQuestions.Application.Questions.Exceptions;

public class QuestionsNotFoundException : NotFoundException
{
    protected QuestionsNotFoundException(Error[] errors) 
        : base(errors)
    {
    }
}