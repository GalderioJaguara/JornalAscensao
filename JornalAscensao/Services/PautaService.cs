using System.Linq.Expressions;
using System.Net;
using JornalAscensao.Data;
using JornalAscensao.Models;
using JornalAscensao.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace JornalAscensao.Services;

public class PautaService(AppDbContext context, ILogger<PautaService> logger, IUsuarioService usuarioService) : IPautaService
{
    public async Task<Pagination<PautaViewModel>> GetPautasAsync(int pageIndex)
    {
        var query = from pautas in context.Pautas
            join usuarios in context.Users
                on pautas.UsuarioId equals usuarios.Id
                where pautas.Fechada == false
            select new PautaViewModel
            {
                Id = pautas.Id,
                Descricao = pautas.Descricao,
                Titulo = pautas.Titulo,
                Imagem = pautas.Imagem,
                Categoria = pautas.Categoria,
                Tipo = pautas.Tipo,
                Fechado = pautas.Fechada,
                LinkConteudo = pautas.LinkConteudo,
                Criado = pautas.Criado,
                UsuarioId =  pautas.UsuarioId,
                UsuarioApelido =  usuarios.Apelido,
            };

        var data = await Pagination<PautaViewModel>.GetItemsPaginados(query, pageIndex, 2);
                
        return data;
    }

    public async Task<PautaViewModel?> GetPautaAsync(Guid id)
    {
        var pauta = await context.Pautas.FindAsync(id);

        if (pauta == null)
        {
            return null;
        }

        return new PautaViewModel
        {
            Id = pauta.Id,
            Tipo = pauta.Tipo,
            Categoria = pauta.Categoria,
            Titulo = pauta.Titulo,
            Descricao = pauta.Descricao,
            Imagem = pauta.Imagem,
            LinkConteudo = pauta.LinkConteudo,

        };
    }

    public async Task<PautaFormViewModel> CriarPautaAsync(PautaFormViewModel request)
    {
        var usuarioId =  usuarioService.GetUsuarioId();
        
        var pauta = new Pauta
        {
            Titulo = request.Titulo,
            Tipo = request.Tipo,
            Categoria = request.Categoria,
            Descricao = request.Descricao,
            Imagem = request.Imagem,
            LinkConteudo = request.LinkConteudo,
            UsuarioId = usuarioId,
        };
   
        await context.Pautas.AddAsync(pauta);
        await context.SaveChangesAsync();
        return new PautaFormViewModel
        {
            Titulo = request.Titulo,
            Tipo = request.Tipo,
            Categoria = request.Categoria,
            Descricao = request.Descricao,
            Imagem = request.Imagem,
            LinkConteudo = request.LinkConteudo,
        };
    }

    public async Task<PautaFormViewModel?> AtualizarPautaAsync(Guid id, PautaFormViewModel request)
    {
        var pauta =  await context.Pautas.FindAsync(id);

        if (pauta == null)
        {
            return null;
        }
        
        pauta.UpdatePauta(request);
        await context.SaveChangesAsync();
        return new PautaFormViewModel
        {
            Titulo = request.Titulo,
            Tipo = request.Tipo,
            Categoria = request.Categoria,
            Descricao = request.Descricao,
            Imagem = request.Imagem,
            LinkConteudo = request.LinkConteudo,
        };
    }

    public async Task<bool> FecharPautaAsync(Guid id)
    {
        var pauta = await context.Pautas.FindAsync(id);
        if (pauta == null)
        {
            return false;
        }

        pauta.Fechada = true;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExcluirPautaAsync(Guid id)
    {
        var pauta = await context.Pautas.FindAsync(id);

        if (pauta == null)
        {
            return false;
        }
        context.Pautas.Remove(pauta);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<PautaMetadados> GetPautaMetadadosAsync(string request)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 ...");
        string html = await  client.GetStringAsync(request);
        var doc = new HtmlAgilityPack.HtmlDocument();
        doc.LoadHtml(html);

        string titulo = doc.DocumentNode.SelectSingleNode("//meta[@property='og:title']").GetAttributeValue("content", "");

        if (String.IsNullOrEmpty(titulo))
        {
            titulo = doc.DocumentNode.SelectSingleNode("//head/title").InnerText;
        }
        
        string descricao = doc.DocumentNode.SelectSingleNode("//meta[@property='og:description']").GetAttributeValue("content", "");

        if (String.IsNullOrEmpty(descricao))
        {
            descricao = doc.DocumentNode.SelectSingleNode("//meta[@name='description']").InnerText;
        }

        string img = doc.DocumentNode.SelectSingleNode("//meta[@property='og:image']").GetAttributeValue("content", "");

        if (String.IsNullOrEmpty(img))
        {
            img = "https://priscilagodoy.com/wp-content/uploads/2019/03/de-onde-vem-o-vazio-existencial.jpg";
        }
        return new PautaMetadados
        {
            Titulo = WebUtility.HtmlDecode(titulo),
            Descricao = WebUtility.HtmlDecode(descricao),
            Imagem = img
        };
    }
    
}