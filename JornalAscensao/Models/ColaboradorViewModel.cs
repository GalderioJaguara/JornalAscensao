using JornalAscensao.Dtos;

namespace JornalAscensao.Models;

public class ColaboradorViewModel
{
    public string Apelido { get; set; }
    public int PautasMes { get; set; }
    public int ArtigoPublicados { get; set; }
    public int PautasSemana { get; set; }

    public IEnumerable<ArtigoViewModel>? ArtigosPublicadosLista { get; set; }
    
    public IEnumerable<ArtigoPendenteDto>? ArtigosPendentesLista { get; set; }
}