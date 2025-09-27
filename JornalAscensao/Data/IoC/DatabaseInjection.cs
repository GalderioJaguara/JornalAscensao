using Microsoft.EntityFrameworkCore;

namespace JornalAscensao.Data.IoC;

public static class DatabaseInjection
{
    public static IServiceCollection AdicionarBancoDeDados(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")).UseSnakeCaseNamingConvention();
        });
        return services;
    }
}