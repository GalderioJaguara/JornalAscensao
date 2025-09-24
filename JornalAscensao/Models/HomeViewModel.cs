namespace JornalAscensao.Models;

public class HomeViewModel
{
    public IEnumerable<ArtigoHomeViewModel> UltimosArtigos { get; set; }
    public ArtigoHomeViewModel ArtigoPrincipal { get; set; }
    public IEnumerable<ArtigoHomeViewModel> ArtigosPolitica { get; set; }
    public IEnumerable<ArtigoHomeViewModel> ArtigosEconomia { get; set; }
    public IEnumerable<ArtigoHomeViewModel> ArtigosMundo { get; set; }


}