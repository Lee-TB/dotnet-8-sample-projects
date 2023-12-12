
using Microsoft.EntityFrameworkCore;
using TodoApi.Entities;

namespace TodoApi.Repositories;

public class InMemoryTodoRepository : ITodoRepository
{
    private readonly TodoDb _db;

    public InMemoryTodoRepository(TodoDb db) => this._db = db;

    public async Task<IEnumerable<Todo>> GetAllAsync()
    {
        return await this._db.Todos.ToListAsync();
    }

    public async Task<Todo?> GetAsync(int id)
    {
        return await this._db.Todos.FindAsync(id);
    }

    public async Task CreateAsync(Todo todo)
    {
        this._db.Todos.Add(todo);
        await this._db.SaveChangesAsync();
    }


    public async Task UpdateAsync(Todo todo)
    {
        this._db.Todos.Update(todo);
        await this._db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Todo todo)
    {
        this._db.Todos.Remove(todo);
        await this._db.SaveChangesAsync();
    }


}