using JornalAscensao.Models;
using Microsoft.AspNetCore.Identity;

namespace JornalAscensao.Data.IoC;

public static class IdentityInjection
{
    public static IServiceCollection AdicionarIdentity(this IServiceCollection services)
    {
        services.AddIdentity<Usuario, IdentityRole>(options =>
        {
            options.Password = new PasswordOptions
            {
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
                RequireNonAlphanumeric = false,
                RequiredLength = 8
            };
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders()
        .AddRoles<IdentityRole>();
        return services;
    }
}