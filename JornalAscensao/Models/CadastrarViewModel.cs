using System.ComponentModel.DataAnnotations;

namespace JornalAscensao.Models;

public class CadastrarViewModel
{
    [Required, StringLength(50, MinimumLength = 3, ErrorMessage = "O apelido deve ter no mínimo 3 caracteres e no máximo 50")]
    public string? Apelido { get; set; }
    
    [Required(ErrorMessage = "Email é obrigatório"), EmailAddress(ErrorMessage = "Email ou senha Inválido/a, por favor tente novamente.")]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "A senha é obrigatória"), StringLength(50, MinimumLength = 8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres"), DataType(DataType.Password)]
    public string? Senha { get; set; }
    
    [DataType(DataType.Password), Required(ErrorMessage = "Senha é obrigatória"), Compare("Senha", ErrorMessage = "As duas senhas devem ser iguais")]
    public string? ConfirmarSenha { get; set; }
}