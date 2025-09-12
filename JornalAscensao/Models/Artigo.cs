using System.ComponentModel.DataAnnotations;

namespace JornalAscensao.Models;

public class Artigo : BaseModel
{
    [Required, MinLength(10), MaxLength(256)]
    public string Titulo { get; set; }
    
    [Required, MaxLength(256)]
    public string Gancho { get; set; }
    
    [Required, MinLength(1000), DataType(DataType.MultilineText)]
    public string Texto { get; set; }
    
    [MaxLength(256), DataType(DataType.Url)]
    public string? Imagem { get; set; }
    
    public string? Referencias { get; set; }

    public StatusArtigo Status { get; set; } = StatusArtigo.Escrevendo;

    public bool Aprovado { get; set; }
    
    public string Slug { get; set; }
    
    public DateTime? Publicado { get; set; }
    
    public Guid PautaId { get; set; }
    
    public Pauta Pauta { get; set; }
    
    public string AutorId { get; set; }
    
    public Usuario Usuario { get; set; }
    
    public string? RevisorId { get; set; }
    
    public Usuario Revisor { get; set; }
    
    public void UpdateArtigo(ArtigoFormViewModel model)
    {
        Titulo = model.Titulo;
        Gancho = model.Gancho;
        Texto = model.Texto;
        Imagem = model.Imagem;
        Referencias = model.Referencias;
    } 
}



public enum StatusArtigo
{
    Escrevendo,
    Revisando,
    Corrigindo,
    Publicado
}