using DevQuestions.Web;
using DevQuestions.Web.Middlewares;
using Framework;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Tags;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProgramDependencies();

builder.Services.AddEndpoints(TagsAssembly.Assembly);

var app = builder.Build();

app.UseExceptionMiddleware();

app.UseExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "DevQuestions"));
}

app.MapControllers();

app.MapEndpoints();


app.Run();