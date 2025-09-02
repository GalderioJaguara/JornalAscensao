using JornalAscensao.Models;
using JornalAscensao.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace JornalAscensao.Controllers;

public class CadastrarController(IAutenticacaoService autenticacaoService) : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index([Bind("Apelido, Email, Senha, ConfirmarSenha")] CadastrarViewModel usuario)
    {
        if (ModelState.IsValid)
        {
            var usuarioCadastrado = await autenticacaoService.CadastrarUsuarioAsync(usuario);
            if (usuarioCadastrado.Succeeded) 
                return Redirect($"Home/Index/");
                
        }
        Console.WriteLine(await autenticacaoService.CadastrarUsuarioAsync(usuario));
        return View(usuario);
    }
}