using Microsoft.EntityFrameworkCore;
using TodoApi;
using TodoApi.Endpoints;
using TodoApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TodoDb");
builder.Services.AddSqlServer<TodoDb>(connectionString);
builder.Services.AddScoped<ITodoRepository, EntityFrameworkTodoRepository>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  var todoDb = scope.ServiceProvider.GetRequiredService<TodoDb>();
  todoDb.Database.Migrate();
}

app.MapTodoEndpoints();

app.Run();
