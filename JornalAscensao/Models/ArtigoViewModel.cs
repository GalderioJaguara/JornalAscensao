using System.ComponentModel.DataAnnotations;

namespace JornalAscensao.Models;

public class ArtigoViewModel
{
    public Guid? Id { get; set; }
    
    [Required, MinLength(10)]
    public string Titulo { get; set; }
    
    [Required, MaxLength(255)]
    public string Gancho { get; set; }
    
    
    [Required, MinLength(1000), DataType(DataType.MultilineText)]
    public string Texto { get; set; }
    
    [MaxLength(255), DataType(DataType.Url)]
    public string? Imagem { get; set; }
    
    public string? Referencias { get; set; }

    public StatusArtigo Status { get; set; } = StatusArtigo.Escrevendo;

    public bool Aprovado { get; set; }
    
    public Guid PautaId { get; set; } = Guid.Empty;
    
    public string? AutorId { get; set; }
    
    public string? AutorApelido { get; set; }
    
    public string? Categoria { get; set; }
    public string? RevisorId { get; set; }
    
    public string? RevisorApelido { get; set; }
    
    public DateTime Publicado { get; set; }
    
    public DateTime? Atualizado { get; set; }
    
    public List<string>? Erros  { get; set; }
}