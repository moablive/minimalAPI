using Microsoft.EntityFrameworkCore;

namespace minimalAPI;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        //Especial Body 
        public DbSet<Tarefa> Tarefas => Set<Tarefa>();
    }