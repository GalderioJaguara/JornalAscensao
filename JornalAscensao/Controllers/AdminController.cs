using JornalAscensao.Models;
using JornalAscensao.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JornalAscensao.Controllers;

public class AdminController(IPautaService pautaService, IArtigoService artigoService, IUsuarioService usuarioService, IAdminService adminService) : Controller
{
    // GET
    [Authorize(Roles = "Admin, Moderador")]
    public async Task<IActionResult> Index()
    {
        var dashboardData = new AdminDashboardViewModel()
        {
            ArtigosFila = await adminService.GetArtigosRevisaoAsync(),
            ArtigosPublicados   = await adminService.GetArtigosPublicadosAsync(),
            PautasAbertas = await adminService.GetPautasStatusAync(),
            PautasFechadas = await adminService.GetPautasStatusAync(true),
            NovosColaboradores = await  adminService.GetNovosColaboradoresAsync(),
            ColaboradoresBloqueados = await adminService.GetColaboradoresBloqueadosAsync()
        };
        return View(dashboardData);
    }

    [Authorize(Roles = "Admin, Moderador")]
    public async Task<IActionResult> Pautas()
    {
        var pautas = await pautaService.GetPautasAsync();
        return View(pautas);
    }

    [Authorize(Roles = "Admin, Moderador")]
    public async Task<IActionResult> Artigos()
    {
        var artigos  = await artigoService.GetArtigosAsync();
        return View(artigos);
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Colaboradores()
    {
        var colaboradores = await usuarioService.GetUsuariosAsync();
        return View(colaboradores);
    }
    
    [Authorize(Roles = "Admin, Moderador")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> FecharPauta(Guid id)
    {
        var pauta = await pautaService.FecharPautaAsync(id);
        if (pauta == false)
        {
            return NotFound();
        }
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExcluirColaborador(string email)
    {
        var usuario = await usuarioService.DeleteUsuarioAsync(email);
        if (usuario == false)
            return NotFound();
        return NoContent();
    }
}