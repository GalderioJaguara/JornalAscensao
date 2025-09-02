namespace JornalAscensao.Models;

public class PautaMetadados
{
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public string? Imagem { get; set; }

    public PautaMetadados(string? titulo = null, string? descricao = null, string? imagem = null)
    {
        Titulo = titulo;
        Descricao = descricao;
        Imagem = imagem;
    }
}