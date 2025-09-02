using System.ComponentModel.DataAnnotations;

namespace JornalAscensao.Models;

public class ArtigoFormViewModel
{
    [Required, MinLength(10)]
    public string Titulo { get; set; }
    
    [Required, MaxLength(255)]
    public string Gancho { get; set; }
    
    [Required, MinLength(1000), DataType(DataType.MultilineText)]
    public string Texto { get; set; }
    
    [MaxLength(255), DataType(DataType.Url)]
    public string? Imagem { get; set; }
    
    public string? Referencias { get; set; }
    public Guid PautaId { get; set; }
    
    public StatusArtigo Status { get; set; }
    
    public bool Aprovado { get; set; }
}