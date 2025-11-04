using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Colaboracion> Colaboracion { get; set; }
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Observacion> Observacion { get; set; }
}
