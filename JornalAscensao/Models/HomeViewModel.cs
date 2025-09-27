namespace JornalAscensao.Models;

public class HomeViewModel
{
    public IEnumerable<ArtigoHomeViewModel> UltimosArtigos { get; set; } = new List<ArtigoHomeViewModel>();
    public ArtigoHomeViewModel? ArtigoPrincipal { get; set; }
    public IEnumerable<ArtigoHomeViewModel> ArtigosPolitica { get; set; } = new List<ArtigoHomeViewModel>();
    public IEnumerable<ArtigoHomeViewModel> ArtigosEconomia { get; set; } = new List<ArtigoHomeViewModel>();
    public IEnumerable<ArtigoHomeViewModel> ArtigosMundo { get; set; } = new List<ArtigoHomeViewModel>();


}