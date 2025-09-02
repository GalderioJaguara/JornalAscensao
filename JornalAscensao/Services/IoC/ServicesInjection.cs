using JornalAscensao.Services.Abstraction;

namespace JornalAscensao.Services.IoC;

public static class ServicesInjection
{
    public static IServiceCollection AdicionarServicos(this IServiceCollection services)
    {
        services.AddScoped<IAutenticacaoService, AutenticacaoService>();
        return services;
    }
}