using System.Security.Claims;
using JornalAscensao.Data;
using JornalAscensao.Dtos;
using JornalAscensao.Models;
using JornalAscensao.Services.Abstraction;
using Microsoft.AspNetCore.Identity;
using JornalAscensao.Models;
using Microsoft.EntityFrameworkCore;

namespace JornalAscensao.Services;

public class UsuarioService(SignInManager<Usuario> signInManager, UserManager<Usuario> userManager,AppDbContext context, ILogger<UsuarioService> logger) : IUsuarioService
{
    
    public async Task<IEnumerable<AdminColaboradoresViewModel>> GetUsuariosAsync()
    {
        var usuarios = await (from usuario in context.Users
            join usuariosRoles in context.UserRoles on usuario.Id equals usuariosRoles.UserId
            join role in context.Roles
                on usuariosRoles.RoleId equals role.Id
            select new AdminColaboradoresViewModel
            {
               Apelido = usuario.Apelido,
               Email = usuario.Email,
               Excluido = usuario.Excluido,
               Criado = usuario.Criado,
               Roles = role.Name
            }).ToListAsync();
        return usuarios;
    }

    public async Task<bool> DeleteUsuarioAsync(string email)
    {
        var usuario = await userManager.FindByEmailAsync(email);
        if (usuario == null)
        {
            return false;
        }
        usuario.Excluido = DateTime.Now;
        await userManager.UpdateAsync(usuario);
        return true;
    }


    public async Task<AuthViewModel> GetUsuarioAsync()
    {
        ClaimsPrincipal principal = signInManager.Context.User;
        var usuarioId = userManager.GetUserId(principal);
        if (usuarioId == null)
            return null;
        
        var usuario = await userManager.FindByIdAsync(usuarioId);
        var roles = await userManager.GetRolesAsync(usuario);
        
        return new AuthViewModel
        {
            Apelido = usuario.Apelido,
            Email = usuario.Email,
            Id = usuario.Id,
            Avatar = usuario.Avatar,
            Roles = roles
        };
    }

    public async Task<AuthViewModel> UpdateUsuarioAsync(UserDto request)
    {
        ClaimsPrincipal principal = signInManager.Context.User;
        var userId = userManager.GetUserId(principal);
        if (userId == null)
        {
            return null;
        }
        var user =  await userManager.FindByIdAsync(userId);
        if (request.Apelido != null)
        {
            user.Apelido = request.Apelido;
        }

        if (request.Email != null)
        {
            user.Email = request.Email;
        }

        if (request.Avatar != null)
        {
            user.Avatar = request.Avatar;
        }
        await userManager.UpdateAsync(user);
        return new AuthViewModel
        {
            Apelido = user.Apelido,
            Email = user.Email,
            Id = user.Id,
            Avatar = user.Avatar,
        };
    }

    public string GetUsuarioId()
    {
        ClaimsPrincipal principal = signInManager.Context.User;
        return userManager.GetUserId(principal);
    }
}