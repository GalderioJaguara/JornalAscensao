using System.ComponentModel.DataAnnotations;

namespace JornalAscensao.Models;

public class ArtigoHomeViewModel
{
    public string Slug { get; set; }
    
    [Required, MinLength(10)]
    public string Titulo { get; set; }
    
    [Required, MaxLength(256)]
    public string Gancho { get; set; }
    
    [MaxLength(255), DataType(DataType.Url)]
    public string? Imagem { get; set; }
    
    public string? AutorApelido { get; set; }
    
    public string? Categoria { get; set; }
    
    public DateTime? Publicado { get; set; }

}