using System.ComponentModel.DataAnnotations;

namespace JornalAscensao.Dtos;

public class UserDto
{
    public string? Apelido { get; set; }
    
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; } 
    
    public string? Avatar { get; set; }
}