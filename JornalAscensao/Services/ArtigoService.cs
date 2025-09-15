using JornalAscensao.Data;
using JornalAscensao.Dtos;
using JornalAscensao.Models;
using JornalAscensao.Utils;
using JornalAscensao.Services.Abstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JornalAscensao.Services;

public class ArtigoService(AppDbContext context, IUsuarioService usuarioService,IPautaService pautaService, UserManager<Usuario> userManager): IArtigoService
{
    public async Task<IEnumerable<ArtigoHomeViewModel>> GetArtigosAsync()
    {
        var artigosQuery = from artigo in context.Artigos
            join autor in context.Users on artigo.AutorId equals autor.Id
            where artigo.Aprovado == true
            select new ArtigoHomeViewModel
            {
                Slug = artigo.Slug,
                Titulo = artigo.Titulo,
                Gancho = artigo.Gancho,
                Imagem = artigo.Imagem,
                Publicado = artigo.Publicado,
                Categoria = artigo.Categoria,
                AutorApelido = autor.Apelido,
            };
        
        return await artigosQuery.ToListAsync(); 
    }

    public async Task<IEnumerable<ArtigoHomeViewModel>> GetArtigosPorCategoriaAsync(string categoria)
    {
        var artigos = context.Artigos.AsNoTracking().Include(a => a.Usuario).AsQueryable();

        if (!string.IsNullOrEmpty(categoria))
        {
            artigos = artigos.Where(a => a.Categoria == categoria);
        }

        return await artigos.Select(a => new ArtigoHomeViewModel
        {
            Titulo = a.Titulo,
            Gancho = a.Gancho,
            Imagem = a.Imagem,
            Publicado = a.Publicado,
            Categoria = a.Categoria,
            AutorApelido = a.Usuario.Apelido,
            Slug = a.Slug
        }).ToListAsync();
    }

    public async Task<IEnumerable<ArtigoViewModel>> GetArtigosColaboradorAsync(string id)
    {
        var usuarioId = usuarioService.GetUsuarioId();
        var artigosQuery = from artigo in context.Artigos
            join autor in context.Users on artigo.AutorId equals autor.Id
            join pauta in context.Pautas on artigo.PautaId equals pauta.Id
            join revisor in context.Users on artigo.RevisorId equals revisor.Id into revisoresGroup
            from revisor in revisoresGroup.DefaultIfEmpty()
            where artigo.Aprovado == true && usuarioId == artigo.AutorId
            select new ArtigoViewModel
            {
                
                Titulo = artigo.Titulo,
                Imagem = artigo.Imagem,
                Publicado = artigo.Publicado ?? artigo.Criado,
                Categoria = pauta.Categoria,
                Slug = artigo.Slug,
                RevisorId = revisor.Id,
                RevisorApelido = revisor.UserName,
                AutorId = autor.Id,
                Referencias = artigo.Referencias,
                AutorApelido = autor.UserName,
            };
        
        return await artigosQuery.ToListAsync(); 
    }

    public async Task<IEnumerable<ArtigoViewModel>> GetArtigosParaRevisarAsync()
    {
        var artigos = context.Artigos.AsNoTracking().Include(p => p.Pauta).Where(a => a.Aprovado == false)
            .Select(x => new ArtigoViewModel
            {
      
                Titulo = x.Titulo,
                Imagem =  x.Imagem,
                Slug = x.Slug,
                Categoria = x.Pauta.Categoria,
                Status = x.Status,
                Aprovado = x.Aprovado
            });
        return await artigos.ToListAsync();
    }

    public async Task<IEnumerable<ArtigoPendenteDto>> GetArtigosPendentesAsync(string id)
    {
        var artigosPendentes =  context.Artigos.AsNoTracking().Where(a => a.Aprovado == false && a.AutorId == id)
            .Select(a => new ArtigoPendenteDto
            {
                Slug = a.Slug,
                LinkImagem = a.Imagem,
                Titulo = a.Titulo,
            });
        return await artigosPendentes.ToListAsync();
    }

    public async Task<ArtigoViewModel?> GetArtigoAsync(string slug)
    {
        var artigo = await context.Artigos.FirstOrDefaultAsync(a => a.Slug == slug);

        if (artigo == null)
        {
            return null;
        }
        
        var revisor = await userManager.FindByIdAsync(artigo.RevisorId);
        var autor = await userManager.FindByIdAsync(artigo.AutorId);
        
        return new ArtigoViewModel
        {
            Titulo = artigo.Titulo,
            Texto = artigo.Texto,
            Gancho = artigo.Gancho,
            Imagem = artigo.Imagem,
            Publicado = artigo.Publicado ?? artigo.Criado,
            Atualizado = artigo.Atualizado,
            Referencias = artigo.Referencias,
            Status = artigo.Status,
            Aprovado = artigo.Aprovado,
            RevisorId = artigo?.RevisorId,
            PautaId = artigo.PautaId,
            AutorApelido = autor?.Apelido,
            RevisorApelido = revisor?.Apelido
        };

    }

    public async Task<ArtigoFormViewModel?> CriarArtigoAsync(ArtigoFormViewModel request)
    {
        var usuarioId =  usuarioService.GetUsuarioId();

        var artigoExiste = await context.Artigos.FirstOrDefaultAsync(a => a.Slug == UrlUtils.UrlFriendlyUtil(request.Titulo));
        if (artigoExiste != null) return null;

        var pauta = await pautaService.GetPautaAsync(request.PautaId);
        if (pauta == null)
            return null;

        pauta.Fechado = true;
        
        var artigo = new Artigo
        {
            Titulo = request.Titulo,
            Gancho = request.Gancho,
            Texto = request.Texto,
            Categoria = pauta.Categoria,
            Referencias = request.Referencias,
            Imagem = request.Imagem,
            PautaId = request.PautaId,
            Slug = UrlUtils.UrlFriendlyUtil(request.Titulo),
            Status = StatusArtigo.Corrigindo,
            AutorId = usuarioId
        };

        await context.Artigos.AddAsync(artigo);
        await context.SaveChangesAsync();

        return new ArtigoFormViewModel
        {
            Titulo = artigo.Titulo,
            Gancho = artigo.Gancho,
            Texto = artigo.Texto,
            Referencias = artigo.Referencias,
            Imagem = artigo.Imagem,
        };
    }

    public async Task<ArtigoFormViewModel?> EditarArtigoAsync(string slug, ArtigoFormViewModel request)
    {
        var artigo = await context.Artigos.FirstOrDefaultAsync(a => a.Slug == slug);
        if (artigo == null)
            return null;
        artigo.UpdateArtigo(request);
        await context.SaveChangesAsync();
        return new ArtigoFormViewModel
        {
            Titulo = artigo.Titulo,
            Gancho = artigo.Gancho,
            Texto = artigo.Texto,
            Referencias = artigo.Referencias,
            Imagem = artigo.Imagem,
        };
    }

    public async Task<bool> DeletarArtigoAsync(string slug)
    {
        var artigo = await context.Artigos.FirstOrDefaultAsync(a => a.Slug == slug);
        if (artigo == null)
            return false;
        context.Artigos.Remove(artigo);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AprovarArtigoAsync(string slug, ArtigoFormViewModel request)
    {
        var artigo  = await context.Artigos.FirstOrDefaultAsync(a => a.Slug == slug);
        if (artigo == null)
            return false;
        artigo.UpdateArtigo(request);
        artigo.Aprovado = request.Aprovado;
        artigo.Status = StatusArtigo.Publicado;
        await context.SaveChangesAsync();
        return true;
    }
}