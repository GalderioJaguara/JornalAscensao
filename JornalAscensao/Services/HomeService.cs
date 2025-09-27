using JornalAscensao.Data;
using JornalAscensao.Models;
using JornalAscensao.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace JornalAscensao.Services;

public class HomeService(AppDbContext context, ILogger<HomeService> logger) : IHomeService
{
    public async Task<HomeViewModel> GetHomeViewModelAsync()
    {
                var artigosQuery = context.Artigos.AsNoTracking().OrderByDescending(a => a.Criado);
        var artigoPrincipal = await artigosQuery.Include(a => a.Usuario).Select(a => new ArtigoHomeViewModel()
        {
            Titulo = a.Titulo ?? string.Empty,
            Slug = a.Slug ?? string.Empty,
            Categoria = a.Categoria ?? string.Empty,
            AutorApelido = a.Usuario.Apelido ?? string.Empty,
            Gancho = a.Gancho ?? string.Empty,
            Imagem = a.Imagem ?? string.Empty,
            Publicado = a.Publicado ?? null 
        }).FirstOrDefaultAsync();

        var artigosPolitica = await artigosQuery.Where(a => a.Categoria == "Politica").Select(a =>
            new ArtigoHomeViewModel()
            {
                Titulo = a.Titulo ?? string.Empty,
                Slug = a.Slug ?? string.Empty,
                Imagem = a.Imagem ?? string.Empty
            }).ToListAsync();

        var artigosEconomia = await artigosQuery.Where(a => a.Categoria == "Economia")
            .Select(a => new ArtigoHomeViewModel()
            {
                Titulo = a.Titulo ?? string.Empty,
                Slug = a.Slug ?? string.Empty,
                Imagem = a.Imagem ?? string.Empty,
            }).ToListAsync();

        var artigosMundo = await artigosQuery.Where(a => a.Categoria == "Mundo").Select(a => new ArtigoHomeViewModel()
        {
            Titulo = a.Titulo ?? string.Empty,
            Imagem = a.Imagem ?? string.Empty,
            Slug = a.Slug ?? string.Empty
        }).ToListAsync();


        var ultimosArtigosQuery = await context.Artigos.GroupBy(a => a.Categoria)

            .Select(a => a.OrderByDescending(o => o.Publicado).First())
            .ToListAsync();

        var ultimosArtigos = ultimosArtigosQuery.Select(a => new ArtigoHomeViewModel()
        {
            Titulo = a.Titulo ?? string.Empty,
            Slug = a.Slug  ?? string.Empty,
            Imagem = a.Imagem  ?? string.Empty,
            Categoria = a.Categoria  ?? string.Empty,
        }).ToList();



        return new HomeViewModel()
        {
            UltimosArtigos = ultimosArtigos ?? new List<ArtigoHomeViewModel>(), 
            ArtigoPrincipal = artigoPrincipal ?? null, 
            ArtigosEconomia = artigosEconomia  ?? new List<ArtigoHomeViewModel>(),
            ArtigosMundo = artigosMundo ?? new List<ArtigoHomeViewModel>(),
            ArtigosPolitica = artigosPolitica ?? new List<ArtigoHomeViewModel>()
        };
        
    }
}