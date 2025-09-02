using JornalAscensao.Models;
using JornalAscensao.Services.Abstraction;
using Microsoft.AspNetCore.Identity;

namespace JornalAscensao.Services;

public class AutenticacaoService(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager) : IAutenticacaoService
{
    public async Task<IdentityResult> CadastrarUsuarioAsync(CadastrarViewModel body)
    {
        var newUser = new Usuario
        {
            Email = body.Email,
            UserName = body.Email,
            Apelido = body.Apelido
        };

        var usuarioCadastro = await userManager.CreateAsync(newUser, body.Senha);
        await userManager.AddToRoleAsync(newUser, "User");
        return usuarioCadastro;
    }

    public async Task<SignInResult> LogarUsuarioAsync(LoginViewModel body)
    {
        var usuario = await userManager.FindByEmailAsync(body.Email);
        if (usuario == null)
            return SignInResult.Failed;

        var resultado =
            await signInManager.PasswordSignInAsync(usuario.Email, body.Senha, body.LembrarDeMim, false);

        return resultado;
    }

    public async Task DeslogarUsuarioAsync()
    {
        await signInManager.SignOutAsync();
    }
}