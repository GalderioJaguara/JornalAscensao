using JornalAscensao.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JornalAscensao.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<Usuario> (options)
{
    public DbSet<Pauta> Pautas { get; set; }
    public DbSet<Artigo> Artigos { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Artigo>()
            .HasOne(a => a.Usuario)
            .WithMany()
            .HasForeignKey(a => a.AutorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Artigo>()
            .HasOne(a => a.Revisor)
            .WithMany()
            .HasForeignKey(a => a.RevisorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Artigo>()
            .HasIndex(a => a.Slug)
            .IsUnique();
    }
}