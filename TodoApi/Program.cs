using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

var todoitems = app.MapGroup("/todoitems");

todoitems.MapGet("/", GetAllTodos);

todoitems.MapGet("/complete", GetCompleteTodos);

todoitems.MapGet("/{id}", GetTodo);

todoitems.MapPost("/", CreateTodo);

todoitems.MapPut("/{id}", UpdateTodo);

todoitems.MapDelete("/{id}", DeleteTodo);

app.Run();

static async Task<IResult> GetAllTodos(TodoDb db)
{

  return TypedResults.Ok(await db.Todos.Select(todo => new TodoDTO(todo)).ToArrayAsync());
}

static async Task<IResult> GetCompleteTodos(TodoDb db)
{
  return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).Select(todo => new TodoDTO(todo)).ToListAsync());
}

static async Task<IResult> GetTodo(int id, TodoDb db)
{
  return await db.Todos.FindAsync(id)
      is Todo todo
          ? TypedResults.Ok(new TodoDTO(todo))
          : TypedResults.NotFound();
}

static async Task<IResult> CreateTodo(TodoDTO todoDTO, TodoDb db)
{
  var todo = new Todo()
  {
    Id = todoDTO.Id,
    Name = todoDTO.Name,
    IsComplete = todoDTO.IsComplete,
  };

  db.Todos.Add(todo);
  await db.SaveChangesAsync();

  return TypedResults.Created($"/todoitems/{todo.Id}", todoDTO);
}

static async Task<IResult> UpdateTodo(int id, TodoDTO todoDTO, TodoDb db)
{
  var todo = await db.Todos.FindAsync(id);

  if (todo is null) return TypedResults.NotFound();

  todo.Name = todoDTO.Name;
  todo.IsComplete = todoDTO.IsComplete;

  await db.SaveChangesAsync();

  return TypedResults.NoContent();
}

static async Task<IResult> DeleteTodo(int id, TodoDb db)
{
  if (await db.Todos.FindAsync(id) is Todo todo)
  {
    db.Todos.Remove(todo);
    await db.SaveChangesAsync();
    return TypedResults.NoContent();
  }

  return TypedResults.NotFound();
}

