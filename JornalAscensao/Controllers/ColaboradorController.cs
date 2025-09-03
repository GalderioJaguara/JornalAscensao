using JornalAscensao.Dtos;
using JornalAscensao.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace JornalAscensao.Controllers;

public class ColaboradorController(IUsuarioService usuarioService, IColaboradorService colaboradorService) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var data = await colaboradorService.GetColaboradorDataAsync();
        return View(data);
    }
    
    public async Task<IActionResult> GetColaborador()
    {
        var dadosUsuario = await usuarioService.GetUsuarioAsync();
        return Ok(dadosUsuario);
    }

    public async Task<IActionResult> UpdateColaborador(UserDto request)
    {
        var user = await usuarioService.UpdateUsuarioAsync(request);
        if (String.IsNullOrEmpty(user.Id))
        {
            return BadRequest(user);
        }
        return Ok(user);
    }
}