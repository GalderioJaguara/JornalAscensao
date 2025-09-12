using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace JornalAscensao.Models;

public class Usuario : IdentityUser
{
    public DateTime Criado { get; set; } = DateTime.UtcNow;
    
    public DateTime Atualizado { get; set; } = DateTime.UtcNow;
    
    public DateTime? Excluido { get; set; } 
    
    [Required, StringLength(50, MinimumLength = 3)]
    public string Apelido { get; set; } = String.Empty;
    public string? Avatar { get; set; }
    
}