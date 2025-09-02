namespace JornalAscensao.Models;

public class AuthViewModel
{
    public string? Id { get; set; }
    public string? Apelido { get; set; }
    public string? Avatar { get; set; }
    public string? Email { get; set; }
    public IList<string> Roles { get; set; }
}