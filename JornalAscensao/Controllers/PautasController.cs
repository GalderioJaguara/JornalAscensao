using JornalAscensao.Models;
using JornalAscensao.Services.Abstraction;
using JornalAscensao.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JornalAscensao.Controllers;

public class PautasController(IPautaService pautaService) : Controller
{
    // GET
    public async Task<IActionResult> Index(int page = 1)
    {
        var pautas = await pautaService.GetPautasAsync(page);
        return View(pautas);
    }
    
    public ActionResult Cadastrar()
    {
        var ModelComSelects = new PautaFormViewModel();
        return View(ModelComSelects);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Cadastrar(PautaFormViewModel model)
    {
        if (ModelState.IsValid)
        {
            var novaPauta = await pautaService.CriarPautaAsync(model);

            return Redirect($"Index");
        }
            
        return View(model);
    }
    
    public async Task<ActionResult> Editar(Guid id)
    {
        var pauta = await pautaService.GetPautaAsync(id);
        if (pauta == null)
        {
            TempData["error"] = "Pauta não encontrada";
            return RedirectToAction("Index");
        }
            
        var pautaFormViewModel = new PautaFormViewModel
        {
            Titulo = pauta.Titulo,
            Categoria = pauta.Categoria,
            Descricao = pauta.Descricao,
            Categorias = PautaUtils.GetCategoriasSelectList(),
            Imagem = pauta.Imagem,
            LinkConteudo = pauta.LinkConteudo,
            Tipo = pauta.Tipo,
            Tipos = PautaUtils.GetTipoSelectList()
        };
        return View(pautaFormViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Editar(Guid id, PautaFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var pautaAtualizada = await pautaService.AtualizarPautaAsync(id, model);

        if (pautaAtualizada == null)
        {
            TempData["error"] = "Pauta não Encontrada";
        }
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> GetLinkConteudoInformacao([FromQuery]string url)
    {
        var info = await pautaService.GetPautaMetadadosAsync(url);
        return Ok(info);
    }

    [HttpGet]
    public async Task<IActionResult> GetConteudoPauta([FromQuery] Guid id)
    {
        var pauta = await pautaService.GetPautaAsync(id);
        if (pauta == null)
            return NotFound();
        return  Ok(pauta);
    } 
    
    [Authorize(Roles = "Admin,Moderador")]
    public async Task<ActionResult> Excluir(Guid id)
    {
        var pauta = await pautaService.GetPautaAsync(id);
        return View(pauta);
    }
        
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Excluir")]
    [Authorize(Roles = "Admin,Moderador")]
    public async Task<ActionResult> ConfirmarExcluirPauta(Guid id)
    {
        var isExcluido = await pautaService.ExcluirPautaAsync(id);
        if (isExcluido)
            return RedirectToAction("Index");
        return View("Index");
    }
}