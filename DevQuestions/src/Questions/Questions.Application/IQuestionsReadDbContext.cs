namespace Questions.Application;

public interface IQuestionsReadDbContext
{
    IQueryable<Questions.Domain.Question> ReadQuestions { get;}
}