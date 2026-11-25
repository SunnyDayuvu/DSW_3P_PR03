using DSW_3P_PR03.Models;
using Microsoft.EntityFrameworkCore;

namespace DSW_3P_PR03.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Personaje> Personajes => Set<Personaje>();
}
