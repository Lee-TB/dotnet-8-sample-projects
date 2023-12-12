using Microsoft.EntityFrameworkCore;
using TodoApi;
using TodoApi.Endpoints;
using TodoApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddScoped<ITodoRepository, InMemoryTodoRepository>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

app.MapTodoEndpoints();

app.Run();
