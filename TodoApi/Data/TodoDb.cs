using Microsoft.EntityFrameworkCore;
using TodoApi.Entities;

namespace TodoApi;

public class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options) : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}