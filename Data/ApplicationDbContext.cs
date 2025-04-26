using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using prog_parcial.Models;

namespace prog_parcial.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
        public DbSet<Player> DbSetPlayer{ get; set; }
                public DbSet<Team> DbSetTeam{ get; set; }
                public DbSet<Assignment> DbSetAssignment{ get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Evita que un jugador se repita en el mismo equipo
        modelBuilder.Entity<Assignment>()
            .HasIndex(a => new { a.TeamId, a.PlayerId })
            .IsUnique();

        // Relaciones explícitas (opcional pero recomendado)
        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.Team)
            .WithMany(t => t.Assignments)
            .HasForeignKey(a => a.TeamId);

        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.Player)
            .WithMany(p => p.Assignments)
            .HasForeignKey(a => a.PlayerId);
    }

}
