using JornalAscensao.Services.Abstraction;

namespace JornalAscensao.Services.IoC;

public static class ServicesInjection
{
    public static IServiceCollection AdicionarServicos(this IServiceCollection services)
    {
        services.AddScoped<IAutenticacaoService, AutenticacaoService>();
        services.AddScoped<IPautaService, PautaService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IArtigoService, ArtigoService>();
        services.AddScoped<IColaboradorService, ColaboradorService>();
        services.AddScoped<IAdminService, AdminService>();
        return services;
    }
}