using System.ComponentModel.DataAnnotations;

namespace JornalAscensao.Models;

public class LoginViewModel
{
    [EmailAddress, Required]
    public string? Email { get; set; }
    
    [DataType(DataType.Password), Required]
    public string? Senha { get; set; }
    
    public bool LembrarDeMim { get; set; }
}