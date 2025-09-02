using JornalAscensao.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JornalAscensao.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<Usuario> (options)
{
    public DbSet<Pauta> Pautas { get; set; }
}