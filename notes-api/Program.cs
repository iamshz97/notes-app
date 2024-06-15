using Microsoft.EntityFrameworkCore;
using notes_api.Models;
using notes_api.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};


app.MapGet("/notes", async (AppDbContext db) =>
    await db.Notes.ToListAsync());

app.MapGet("/notes/{id}", async (int id, AppDbContext db) =>
    await db.Notes.FindAsync(id) is Note note
        ? Results.Ok(note)
        : Results.NotFound());

app.MapPost("/notes", async (Note note, AppDbContext db) =>
{
    db.Notes.Add(note);
    await db.SaveChangesAsync();
    return Results.Created($"/notes/{note.Id}", note);
});

app.MapPut("/notes/{id}", async (int id, Note inputNote, AppDbContext db) =>
{
    var note = await db.Notes.FindAsync(id);

    if (note is null)
    {
        return Results.NotFound();
    }

    note.Title = inputNote.Title;
    note.Content = inputNote.Content;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/notes/{id}", async (int id, AppDbContext db) =>
{
    if (await db.Notes.FindAsync(id) is Note note)
    {
        db.Notes.Remove(note);
        await db.SaveChangesAsync();
        return Results.Ok(note);
    }

    return Results.NotFound();
});

app.Run();