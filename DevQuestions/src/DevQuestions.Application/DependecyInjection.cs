using DevQuestions.Application.Abstractions;
using DevQuestions.Application.Questions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DevQuestions.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
       services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
       
       var assembly = typeof(DependencyInjection).Assembly;
       services.Scan(scan => scan.FromAssemblies(assembly)
           .AddClasses(classes => classes
               .AssignableToAny(typeof(IHandler<,>), typeof(IHandler<>)))
           .AsSelfWithInterfaces()
           .WithScopedLifetime());

       return services;
    }
}