// using  DevQuestion.Infrastructure.Postgres;
using FluentValidation;
using Questions.Presenters;
using Shared;
using Tags;

namespace DevQuestions.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services)
    {

        services.AddWebDependencies();
        
        services.AddQuestionsModule();

        services.AddTagsModule();
           
           return services;
                
    }
       
        private static IServiceCollection AddWebDependencies(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddOpenApi();
            
            return services;
        }
}