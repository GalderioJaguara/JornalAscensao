using JornalAscensao.Models;
using JornalAscensao.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JornalAscensao.Controllers;

[Route("artigos")]
public class ArtigosController(IArtigoService artigoService, IUsuarioService usuarioService) : Controller
{
    // GET
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        var artigos = await artigoService.GetArtigosAsync();
        return View(artigos);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult> Artigo(string id)
    {
        var artigo = await artigoService.GetArtigoAsync(id);
        if (artigo != null)
            return View(artigo);
        TempData["error"] = "Artigo não encontrado";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("escrever")]
    public ActionResult Escrever([FromQuery] Guid id)
    {
        var artigo = new ArtigoFormViewModel()
        {
            PautaId = id
        };
        return View(artigo);
    }

    [HttpPost]
    [Route("escrever")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Escrever(ArtigoFormViewModel request)
    {
        if (ModelState.IsValid)
        {
            var artigo = await artigoService.CriarArtigoAsync(request);
            Console.WriteLine(artigo);
            return RedirectToAction("Index");
        }

        return View(request);
    }
    
    [HttpGet]
    [Route("editar/{id}")]
    public async Task<ActionResult> Editar(string id)
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
    [Route("editar/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Editar(string id, ArtigoFormViewModel request)
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
    [HttpGet("excluir/{id}")]
    public async Task<ActionResult> Excluir(string id)
    {
        var artigo = await artigoService.GetArtigoAsync(id);
        return View(artigo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Excluir")]
    [Route("excluir/{id}")]
    [Authorize(Roles = "Admin,Moderador")]
    public async Task<ActionResult> ConfirmarExclusaoDoArtigo(string id)
    {
        var isExcluido = await artigoService.DeletarArtigoAsync(id);
        if (isExcluido == false)
        {
            return View();
        } 
        return RedirectToAction("Index");
    }
    
    [Authorize(Roles = "Admin,Moderador,Revisor")]
    [HttpGet("fila-de-revisao")]
    public async Task<ActionResult> Fila()
    {
        var artigos = await artigoService.GetArtigosParaRevisarAsync();
        return View(artigos);
    }
    
    [Authorize(Roles = "Admin,Moderador,Revisor")]
    [HttpGet("revisar/{id}")]
    public async Task<ActionResult> Revisar(string id)
    {
        var artigo = await artigoService.GetArtigoAsync(id);
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
    public async Task<ActionResult> AprovarArtigo(string id, ArtigoFormViewModel request)
    {
        var isAprovado = await artigoService.AprovarArtigoAsync(id, request);
        if (isAprovado == false)
        {
            TempData["error"] = "Artigo Não Encontrado";
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");
    }
}