using JornalAscensao.Dtos;
using JornalAscensao.Models;

namespace JornalAscensao.Services.Abstraction;

public interface IUsuarioService
{
    public Task<AuthViewModel> GetUsuarioAsync();
    public Task<bool> DeleteUsuarioAsync(string email);
    public Task<AuthViewModel> UpdateUsuarioAsync(UserDto request);
    public string GetUsuarioId();
    public Task<IEnumerable<AdminColaboradoresViewModel>> GetUsuariosAsync();
}