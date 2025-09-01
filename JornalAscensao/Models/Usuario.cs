using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace JornalAscensao.Models;

public class Usuario : IdentityUser
{
    [Required, StringLength(50, MinimumLength = 3)]
    public string Apelido { get; set; } = String.Empty;
    public string? Avatar { get; set; }
}