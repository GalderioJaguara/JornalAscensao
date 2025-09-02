using JornalAscensao.Models;
using Microsoft.AspNetCore.Identity;

namespace JornalAscensao.Services.Abstraction;

public interface IAutenticacaoService
{
    public Task<IdentityResult> CadastrarUsuarioAsync(CadastrarViewModel request);
    public Task<SignInResult> LogarUsuarioAsync(LoginViewModel request);
    public Task DeslogarUsuarioAsync();
}