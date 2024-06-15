using Microsoft.EntityFrameworkCore;
using notes_api.Models;

namespace notes_api.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Note> Notes { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}