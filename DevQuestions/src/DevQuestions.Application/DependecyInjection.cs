using DevQuestions.Application.Abstractions;
using DevQuestions.Application.FulltextSearch;
using DevQuestions.Application.Questions;
using DevQuestions.Application.Questions.CreateQuestion;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DevQuestions.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
       services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
       
       services.AddScoped<ICommandHandler<Guid, CreateQuestionCommand>, CreateQuestionHandler>();
       services.AddScoped<ICommandHandler<Guid, AddAnswerCommand>, AddAnswerHandler>();
       
       
       var assemlie = typeof(DependencyInjection).Assembly;
       services.Scan(scan => scan.FromAssemblies(assemlie)
           .AddClasses(classes => classes
               .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>))));

       return services;
    }
}