using JornalAscensao.Models;
using JornalAscensao.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace JornalAscensao.Controllers;

public class LoginController(IAutenticacaoService autenticacaoService) : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index([Bind("Email, Senha")] LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var usuario = await autenticacaoService.LogarUsuarioAsync(model);
            if (usuario.Succeeded)
                return Redirect($"/Home/");
            ModelState.AddModelError(string.Empty, "Email ou Senha Incorretos, tente novamente.");
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await autenticacaoService.DeslogarUsuarioAsync();
        return Redirect("/");
    }
}