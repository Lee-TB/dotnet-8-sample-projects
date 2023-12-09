using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

var todoitems = app.MapGroup("/todoitems");

todoitems.MapGet("/", async (TodoDb db) =>
{
  return await db.Todos.ToListAsync();
});

todoitems.MapGet("/complete", async (TodoDb db) =>
{
  return await db.Todos.Where(todo => todo.IsComplete).ToListAsync();
});

todoitems.MapGet("/{id}", async (int id, TodoDb db) =>
{
  return await db.Todos.FindAsync(id) is Todo todo ? Results.Ok(todo) : Results.NotFound();
});

todoitems.MapPost("", async (Todo todo, TodoDb db) =>
{
  db.Todos.Add(todo);
  await db.SaveChangesAsync();
  return Results.Created($"/{todo.Id}", todo);
});


todoitems.MapPut("/{id}", async (int id, Todo inputTodo, TodoDb db) =>
{
  var todo = await db.Todos.FindAsync(id);

  if (todo is null) return Results.NotFound();

  todo.Name = inputTodo.Name;
  todo.IsComplete = inputTodo.IsComplete;

  await db.SaveChangesAsync();

  return Results.NoContent();
});


todoitems.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
  if (await db.Todos.FindAsync(id) is Todo todo)
  {
    db.Todos.Remove(todo);
    await db.SaveChangesAsync();
    return Results.NoContent();
  }

  return Results.NotFound();
});

app.Run();
