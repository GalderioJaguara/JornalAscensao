using Microsoft.AspNetCore.Identity;

namespace JornalAscensao.Models;

public class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = { "Admin", "User", "Moderador", "Revisor" };


        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

        }

        var userManager = serviceProvider.GetRequiredService<UserManager<Usuario>>();
        var users = new List<Usuario>
        {
            new Usuario
            {
                Email = "teste@email.com",
                UserName = "teste@email.com",
                Apelido = "Admin"
            },
            new Usuario
            {
                Email = "comum@email.com",
                UserName = "comum@email.com",
                Apelido = "Usuario"
            },
            new Usuario
            {
                Email = "moderador@email.com",
                UserName = "moderador@email.com",
                Apelido = "Moderador"
            },
            new Usuario
            {
                Email = "revisor@email.com",
                UserName = "revisor@email.com",
                Apelido = "Revisor"
            }
        };
        for (int i = 0; i < users.Count; i++)
        {
            if (await userManager.FindByEmailAsync(users[i].Email) == null)
            {
                await userManager.CreateAsync(users[i], "P4ssw@rd");
                switch (i)
                {
                    case 0:
                        await userManager.AddToRoleAsync(users[i], "Admin");
                        break;
                    case 1:
                        await userManager.AddToRoleAsync(users[i], "User");
                        break;
                    case 2:
                        await userManager.AddToRoleAsync(users[i], "Moderador");
                        break;
                    case 3:
                        await userManager.AddToRoleAsync(users[i], "Revisor");
                        break;
                    default:
                        continue;
                }
            }
        }
    }
}