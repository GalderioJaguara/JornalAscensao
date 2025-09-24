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
            Titulo = a.Titulo,
            Slug = a.Slug,
            Categoria = a.Categoria,
            AutorApelido = a.Usuario.Apelido,
            Gancho = a.Gancho,
            Imagem = a.Imagem,
            Publicado = a.Publicado
        }).FirstOrDefaultAsync();

        var artigosPolitica = await artigosQuery.Where(a => a.Categoria == "Politica").Select(a =>
            new ArtigoHomeViewModel()
            {
                Titulo = a.Titulo,
                Slug = a.Slug,
                Imagem = a.Imagem
            }).ToListAsync();

        var artigosEconomia = await artigosQuery.Where(a => a.Categoria == "Economia")
            .Select(a => new ArtigoHomeViewModel()
            {
                Titulo = a.Titulo,
                Slug = a.Slug,
                Imagem = a.Imagem,
            }).ToListAsync();

        var artigosMundo = await artigosQuery.Where(a => a.Categoria == "Mundo").Select(a => new ArtigoHomeViewModel()
        {
            Titulo = a.Titulo,
            Imagem = a.Imagem,
            Slug = a.Slug
        }).ToListAsync();


        var ultimosArtigosQuery = await context.Artigos.GroupBy(a => a.Categoria)

            .Select(a => a.OrderByDescending(o => o.Publicado).First())
            .ToListAsync();

        var ultimosArtigos = ultimosArtigosQuery.Select(a => new ArtigoHomeViewModel()
        {
            Titulo = a.Titulo,
            Slug = a.Slug,
            Imagem = a.Imagem,
            Categoria = a.Categoria
        }).ToList();



    return new HomeViewModel()
        {
            UltimosArtigos = ultimosArtigos, 
            ArtigoPrincipal = artigoPrincipal, 
            ArtigosEconomia = artigosEconomia,
            ArtigosMundo = artigosMundo,
            ArtigosPolitica = artigosPolitica
        };
        
    }
}