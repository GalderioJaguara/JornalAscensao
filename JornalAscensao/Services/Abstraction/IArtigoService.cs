using JornalAscensao.Dtos;
using JornalAscensao.Models;

namespace JornalAscensao.Services.Abstraction;

public interface IArtigoService
{
    public Task<IEnumerable<ArtigoHomeViewModel>> GetArtigosAsync();
    public Task<IEnumerable<ArtigoHomeViewModel>> GetArtigosPorCategoriaAsync(string categoria);
    public Task<IEnumerable<ArtigoViewModel>> GetArtigosColaboradorAsync(string id);
    public Task<IEnumerable<ArtigoViewModel>> GetArtigosParaRevisarAsync();
    public Task<IEnumerable<ArtigoPendenteDto>> GetArtigosPendentesAsync(string id);
    public Task<ArtigoViewModel?> GetArtigoAsync(string slug);
    public Task<ArtigoFormViewModel?> CriarArtigoAsync(ArtigoFormViewModel request);
    public Task<ArtigoFormViewModel?> EditarArtigoAsync(string slug,ArtigoFormViewModel request);
    public Task<bool> DeletarArtigoAsync(string slug);
    public Task<bool> AprovarArtigoAsync(string slug, ArtigoFormViewModel request);
}