using JornalAscensao.Data;
using JornalAscensao.Dtos;
using JornalAscensao.Models;
using JornalAscensao.Services.Abstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JornalAscensao.Services;

public class ArtigoService(AppDbContext context, IUsuarioService usuarioService, UserManager<Usuario> userManager): IArtigoService
{
    public async Task<IEnumerable<ArtigoHomeViewModel>> GetArtigosAsync()
    {
        var artigosQuery = from artigo in context.Artigos
            join autor in context.Users on artigo.AutorId equals autor.Id
            join pauta in context.Pautas on artigo.PautaId equals pauta.Id
            where artigo.Aprovado == true
            select new ArtigoHomeViewModel
            {
                Id = artigo.Id,
                Titulo = artigo.Titulo,
                Gancho = artigo.Gancho,
                Imagem = artigo.Imagem,
                Publicado = artigo.Publicado,
                Categoria = pauta.Categoria,
                AutorApelido = autor.UserName,
            };
        
        return await artigosQuery.ToListAsync(); 
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
                Id = artigo.Id,
                Titulo = artigo.Titulo,
                Imagem = artigo.Imagem,
                Publicado = artigo.Publicado ?? artigo.Criado,
                Categoria = pauta.Categoria,
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
                Id = x.Id,
                Titulo = x.Titulo,
                Imagem =  x.Imagem,
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
                Id = a.Id,
                LinkImagem = a.Imagem,
                Titulo = a.Titulo,
            });
        return await artigosPendentes.ToListAsync();
    }

    public async Task<ArtigoViewModel?> GetArtigoAsync(Guid id)
    {
        var artigo = await context.Artigos.FindAsync(id);

        if (artigo == null)
        {
            return null;
        }
        
        var revisor = await userManager.FindByIdAsync(artigo.RevisorId);
        var autor = await userManager.FindByIdAsync(artigo.AutorId);
        
        return new ArtigoViewModel
        {
            Id = artigo.Id,
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

    public async Task<ArtigoFormViewModel> CriarArtigoAsync(ArtigoFormViewModel request)
    {
        var usuarioId =  usuarioService.GetUsuarioId();
        
        var artigo = new Artigo
        {
            Titulo = request.Titulo,
            Gancho = request.Gancho,
            Texto = request.Texto,
            Referencias = request.Referencias,
            Imagem = request.Imagem,
            PautaId = request.PautaId,
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

    public async Task<ArtigoFormViewModel?> EditarArtigoAsync(Guid id, ArtigoFormViewModel request)
    {
        var artigo = await context.Artigos.FindAsync(id);
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

    public async Task<bool> DeletarArtigoAsync(Guid id)
    {
        var artigo = await context.Artigos.FindAsync(id);
        if (artigo == null)
            return false;
        context.Artigos.Remove(artigo);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AprovarArtigoAsync(Guid id, ArtigoFormViewModel request)
    {
        var artigo  = await context.Artigos.FindAsync(id);
        if (artigo == null)
            return false;
        artigo.UpdateArtigo(request);
        artigo.Aprovado = request.Aprovado;
        artigo.Status = StatusArtigo.Publicado;
        await context.SaveChangesAsync();
        return true;
    }
}