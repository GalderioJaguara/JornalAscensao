using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JornalAscensao.Models;
using JornalAscensao.Services.Abstraction;

namespace JornalAscensao.Controllers;

public class HomeController(ILogger<HomeController> logger, IHomeService homeService)  : Controller
{
   
    public async Task<IActionResult> Index()
    {
        try
        {
            var homeData = await homeService.GetHomeViewModelAsync();
            return View(homeData);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return View(new HomeViewModel());
            
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}