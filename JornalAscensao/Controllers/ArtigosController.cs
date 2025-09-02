using JornalAscensao.Models;
using JornalAscensao.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JornalAscensao.Controllers;

public class ArtigosController(IArtigoService artigoService, IUsuarioService usuarioService) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var artigos = await artigoService.GetArtigosAsync();
        return View(artigos);
    }
    
    public async Task<ActionResult> Artigo(Guid id)
    {
        var artigo = await artigoService.GetArtigoAsync(id);
        if (artigo != null)
            return View(artigo);
        TempData["error"] = "Artigo não encontrado";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public ActionResult Escrever()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Escrever(Guid id, ArtigoFormViewModel request)
    {
        if (ModelState.IsValid)
        {
            request.PautaId = id;
            var artigo = await artigoService.CriarArtigoAsync(request);
            Console.WriteLine(artigo);
            return RedirectToAction("Index");
        }

        return View(request);
    }
    
    public async Task<ActionResult> Editar(Guid id)
    {
        var usuario = await usuarioService.GetUsuarioAsync();
        var artigo = await artigoService.GetArtigoAsync(id);

        if (String.IsNullOrEmpty(usuario.Id) || usuario.Roles.Any(roles => roles == "Admin" || roles == "Moderador")
                                             || !artigo.Aprovado || usuario.Id == artigo.AutorId)
        {
            var artigoViewModel = new ArtigoFormViewModel
            {
                Titulo = artigo.Titulo,
                Gancho = artigo.Gancho,
                Texto = artigo.Texto,
                Referencias = artigo.Referencias,
                Imagem = artigo.Imagem,
                PautaId = artigo.PautaId,
                Status = artigo.Status,
                Aprovado = artigo.Aprovado
            };
            return View(artigoViewModel);
        }
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Editar(Guid id, ArtigoFormViewModel request)
    {
        
        if (ModelState.IsValid == false)
        {
            return View(request);
        }
        var artigo = await artigoService.EditarArtigoAsync(id, request);
        if (artigo == null)
        {
            TempData["error"] = "Artigo não Encontrado";
            RedirectToAction("Index");
        }
        
        return RedirectToAction("Index");
    }
    
    [Authorize(Roles = "Admin,Moderador")]
    public async Task<ActionResult> Excluir(Guid id)
    {
        var artigo = await artigoService.GetArtigoAsync(id);
        return View(artigo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Excluir")]
    [Authorize(Roles = "Admin,Moderador")]
    public async Task<ActionResult> ConfirmarExclusaoDoArtigo(Guid id)
    {
        var isExcluido = await artigoService.DeletarArtigoAsync(id);
        if (isExcluido == false)
        {
            return View();
        } 
        return RedirectToAction("Index");
    }
    
    [Authorize(Roles = "Admin,Moderador,Revisor")]
    public async Task<ActionResult> Fila()
    {
        var artigos = await artigoService.GetArtigosParaRevisarAsync();
        return View(artigos);
    }
    
    [Authorize(Roles = "Admin,Moderador,Revisor")]
    public async Task<ActionResult> Revisar(Guid id)
    {
        var artigo = await artigoService.GetArtigoAsync(id);
        Console.Write(artigo.Id);
        if (artigo == null)
        {
            TempData["error"] = "Artigo não encontrado";
            return RedirectToAction("Index");
        } 
        return  View(artigo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize (Roles = "Admin,Moderador,Revisor")]
    public async Task<ActionResult> AprovarArtigo(Guid id, ArtigoFormViewModel request)
    {
        Console.WriteLine(id);
        var isAprovado = await artigoService.AprovarArtigoAsync(id, request);
        if (isAprovado == false)
        {
            TempData["error"] = "Artigo Não Encontrado";
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");
    }
}