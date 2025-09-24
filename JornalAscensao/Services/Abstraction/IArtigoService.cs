using JornalAscensao.Dtos;
using JornalAscensao.Models;

namespace JornalAscensao.Services.Abstraction;

public interface IArtigoService
{
    public Task<Pagination<ArtigoHomeViewModel>> GetArtigosAsync(int pageNumber);
    public Task<Pagination<ArtigoHomeViewModel>> GetArtigosPorCategoriaAsync(string categoria, int pageIndex);
    public Task<IEnumerable<ArtigoViewModel>> GetArtigosColaboradorAsync(string id);
    public Task<Pagination<ArtigoViewModel>> GetArtigosParaRevisarAsync(int pageIndex);
    public Task<IEnumerable<ArtigoPendenteDto>> GetArtigosPendentesAsync(string id);
    public Task<ArtigoViewModel?> GetArtigoAsync(string slug);
    public Task<ArtigoFormViewModel?> CriarArtigoAsync(ArtigoFormViewModel request);
    public Task<ArtigoFormViewModel?> EditarArtigoAsync(string slug,ArtigoFormViewModel request);
    public Task<bool> DeletarArtigoAsync(string slug);
    public Task<bool> AprovarArtigoAsync(string slug, ArtigoFormViewModel request);
}