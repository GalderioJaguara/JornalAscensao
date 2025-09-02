using System.ComponentModel.DataAnnotations;

namespace JornalAscensao.Models;

public class Pauta : BaseModel
{
    [Required, MaxLength(256)] public string Tipo { get; set; } = String.Empty;

    [Required, MaxLength(256)] public string Categoria { get; set; } = String.Empty;

    [Required, MaxLength(256), MinLength(10)]
    public string Titulo { get; set; } = String.Empty;

    [Required, MinLength(30)] public string Descricao { get; set; } = String.Empty;

    [MaxLength(256)]
    public string? Imagem { get; set; }

    [Required, DataType(DataType.Url), MaxLength(256)]
    public string LinkConteudo { get; set; } = String.Empty;

    public bool Fechada { get; set; } = false;
    
    [Required, MaxLength(450)]
    public string UsuarioId { get; set; }
    
    public Usuario Usuario { get; set; }

    public void UpdatePauta(PautaFormViewModel pauta)
    {
        Categoria = pauta.Categoria;
        Titulo = pauta.Titulo;
        Descricao = pauta.Descricao;
        Imagem = pauta.Imagem;
        Atualizar();
    }
}