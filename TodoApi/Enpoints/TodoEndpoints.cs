using Microsoft.EntityFrameworkCore;
using TodoApi.Entities;
using TodoApi.Repositories;

namespace TodoApi.Endpoints;

public static class TodoEndpoints
{
    public static RouteGroupBuilder MapTodoEndpoints(this IEndpointRouteBuilder routes)
    {
        var todoitems = routes.MapGroup("/todoitems");

        todoitems.MapGet("/", GetAllTodos);

        todoitems.MapGet("/complete", GetCompleteTodos);

        todoitems.MapGet("/{id}", GetTodo);

        todoitems.MapPost("/", CreateTodo);

        todoitems.MapPut("/{id}", UpdateTodo);

        todoitems.MapDelete("/{id}", DeleteTodo);

        return todoitems;
    }

    static async Task<IResult> GetAllTodos(ITodoRepository repository)
    {
        var todoDTOs = (await repository.GetAllAsync()).Select(todo => todo.AsDTO());
        return TypedResults.Ok(todoDTOs);
    }

    static async Task<IResult> GetCompleteTodos(ITodoRepository repository)
    {
        var todoDTOs = (await repository.GetAllAsync())
                        .Select(todo => todo.AsDTO())
                        .Where(todo => todo.IsComplete);
        return TypedResults.Ok(todoDTOs);
    }

    static async Task<IResult> GetTodo(int id, ITodoRepository repository)
    {
        return await repository.GetAsync(id)
            is Todo todo
                ? TypedResults.Ok(todo.AsDTO())
                : TypedResults.NotFound();
    }

    static async Task<IResult> CreateTodo(TodoDTO todoDTO, ITodoRepository repository)
    {
        var todo = new Todo()
        {
            Name = todoDTO.Name,
            IsComplete = todoDTO.IsComplete,
        };

        await repository.CreateAsync(todo);

        return TypedResults.Created($"/todoitems/{todo.Id}", todo.AsDTO());
    }

    static async Task<IResult> UpdateTodo(int id, TodoDTO todoDTO, ITodoRepository repository)
    {
        var todo = await repository.GetAsync(id);

        if (todo is null) return TypedResults.NotFound();

        todo.Name = todoDTO.Name;
        todo.IsComplete = todoDTO.IsComplete;

        await repository.UpdateAsync(todo);

        return TypedResults.NoContent();
    }

    static async Task<IResult> DeleteTodo(int id, ITodoRepository repository)
    {
        if (await repository.GetAsync(id) is Todo todo)
        {
            await repository.DeleteAsync(todo);
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }
}