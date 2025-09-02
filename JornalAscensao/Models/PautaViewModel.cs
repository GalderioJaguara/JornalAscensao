using System.ComponentModel.DataAnnotations;

namespace JornalAscensao.Models;

public class PautaViewModel
{
    public Guid? Id { get; set; }
    public DateTime Criado { get; set; }
    
    [Required]
    public string Tipo { get; set; } = String.Empty;
    
    [Required]
    public string Categoria { get; set; } = String.Empty;
    
    [Required, StringLength(255, MinimumLength = 5, ErrorMessage = "Titulo deve ter no mínimo 5 caracteres e no máximo 50")]
    public string Titulo  { get; set; } = String.Empty;
    
    [Required, MinLength(30)]
    public string Descricao  { get; set; } = String.Empty;
    
    public string? Imagem  { get; set; } 
    
    [Required, DataType(DataType.Url)]
    public string LinkConteudo { get; set; } = String.Empty;
    
    public string? UsuarioId { get; set; }
    public string? UsuarioApelido  { get; set; }
}