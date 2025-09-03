using System.Security.Claims;
using JornalAscensao.Data;
using JornalAscensao.Models;
using JornalAscensao.Services.Abstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JornalAscensao.Services;

public class ColaboradorService(AppDbContext context, UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IArtigoService artigoService) : IColaboradorService
{
    public async Task<ColaboradorViewModel> GetColaboradorDataAsync()
    {
        var hj = DateTime.UtcNow;
        var inicioDoMes = new DateTime(hj.Year, hj.Month, 1, 0,0,0, DateTimeKind.Utc);
        var finalDoMes = inicioDoMes.AddMonths(1);
        
        ClaimsPrincipal principal = signInManager.Context.User;
        var usuario = await userManager.FindByIdAsync(userManager.GetUserId(principal));
        
        var pautaMes = context.Pautas.AsNoTracking().Where(p => p.UsuarioId == usuario.Id && p.Criado >= inicioDoMes && p.Criado < finalDoMes).Count();
        var artigosPublicados = context.Artigos.AsNoTracking()
            .Where(a => a.AutorId == usuario.Id && a.Aprovado == true).Count();

        

        var domingo = hj.AddDays(-(int)hj.DayOfWeek);

        var proximoDomingo = domingo.AddDays(7);

        var pautasSemana = context.Pautas.Where(x => x.Criado >= domingo && x.Criado < proximoDomingo && x.UsuarioId == usuario.Id).Count();

        var artigosPublicadosLista = await artigoService.GetArtigosColaboradorAsync(usuario.Id);

        var artigosPendentes = await artigoService.GetArtigosPendentesAsync(usuario.Id);

        
        return new ColaboradorViewModel
        {
            Apelido = usuario.Apelido,
            PautasMes = pautaMes,
            ArtigoPublicados = artigosPublicados,
            PautasSemana = pautasSemana,
            ArtigosPendentesLista = artigosPendentes,
            ArtigosPublicadosLista = artigosPublicadosLista,
        };
    }
}