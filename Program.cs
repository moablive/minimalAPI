using Microsoft.EntityFrameworkCore;
using minimalAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DB MEMORY
builder.Services.AddDbContext<AppDbContext>(
    opt => opt.UseInMemoryDatabase("TarefasDB")
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Minimal API
app.MapGet("/tarefas", async (AppDbContext db) => 
    await db.Tarefas.ToListAsync() 
);

app.MapGet("/tarefas/{id}", async (AppDbContext db, int id) =>  
    await db.Tarefas.FindAsync(id)
    is Tarefa tarefa ? Results.Ok(tarefa) : Results.NotFound()
);

app.MapGet("/tarefas/concluida", async (AppDbContext db) =>
    await db.Tarefas.Where(t => t.Concluida == true).ToListAsync()
);

app.MapPost("/tarefas", async(Tarefa tarefa, AppDbContext db) =>
    {
        db.Tarefas.Add(tarefa);
        await db.SaveChangesAsync();
        return Results.Created($"/tarefas/{tarefa.Id}", tarefa);
    }
);

app.MapPut("/tarefas/{id}", async(int id, Tarefa inputTarefa, AppDbContext db) =>
    {
        var tarefa = await db.Tarefas.FindAsync(id);

        if (tarefa == null)
        {
            return Results.NotFound();
        }

        tarefa.Nome = inputTarefa.Nome;
        tarefa.Concluida = inputTarefa.Concluida;

        await db.SaveChangesAsync();
        return Results.NoContent();
    }
);

app.MapDelete("/tarefas/{id}", async(int id, AppDbContext db) =>
    {
        if(await db.Tarefas.FindAsync(id) is Tarefa tarefa)
        {
            db.Tarefas.Remove(tarefa);
            await db.SaveChangesAsync();
            return Results.Ok(tarefa);
        }
        return Results.NotFound();
    }
);

app.Run();