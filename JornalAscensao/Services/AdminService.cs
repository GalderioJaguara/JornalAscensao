using JornalAscensao.Data;
using JornalAscensao.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace JornalAscensao.Services;

public class AdminService(AppDbContext context) : IAdminService
{
    
    public async Task<int> GetPautasStatusAync(bool status)
    {
        var pautaStatus = context.Pautas.AsNoTracking().AsQueryable();
       
        if (status)
        {
           return await pautaStatus.Where(p => p.Fechada == true).CountAsync();
        }
       return await pautaStatus.Where(p => p.Fechada == false).CountAsync();
    }

    public async Task<int> GetArtigosPublicadosAsync()
    {
        var artigosPublicados = context.Artigos.Where(a => a.Aprovado == true);
        return await artigosPublicados.CountAsync();
    }

    public async Task<int> GetArtigosRevisaoAsync()
    {
        var artigosRevisao = context.Artigos.Where(a => a.Aprovado == false);
        return await artigosRevisao.CountAsync();
    }

    public async Task<int> GetNovosColaboradoresAsync()
    {
        var diaAtual = DateTime.UtcNow;
        var domingo = diaAtual.AddDays(-(int)diaAtual.DayOfWeek);
        var proximoDomungo = domingo.AddDays(7);

        var novosColaboradores = await context.Users.Where(u => u.Criado >= domingo && u.Criado < proximoDomungo).CountAsync();
        return novosColaboradores;
    }

    public async Task<int> GetColaboradoresBloqueadosAsync()
    {
        var usuariosBloqueados = context.Users.Where(u => u.LockoutEnd != null && u.LockoutEnd > DateTime.UtcNow);
        return await usuariosBloqueados.CountAsync();
    }
}