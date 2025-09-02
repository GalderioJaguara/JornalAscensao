using JornalAscensao.Dtos;
using JornalAscensao.Models;

namespace JornalAscensao.Services.Abstraction;

public interface IArtigoService
{
    public Task<IEnumerable<ArtigoHomeViewModel>> GetArtigosAsync();
    public Task<IEnumerable<ArtigoViewModel>> GetArtigosColaboradorAsync(string id);
    public Task<IEnumerable<ArtigoViewModel>> GetArtigosParaRevisarAsync();
    public Task<IEnumerable<ArtigoPendenteDto>> GetArtigosPendentesAsync(string id);
    public Task<ArtigoViewModel?> GetArtigoAsync(Guid id);
    public Task<ArtigoFormViewModel> CriarArtigoAsync(ArtigoFormViewModel request);
    public Task<ArtigoFormViewModel?> EditarArtigoAsync(Guid id,ArtigoFormViewModel request);
    public Task<bool> DeletarArtigoAsync(Guid id);
    public Task<bool> AprovarArtigoAsync(Guid id, ArtigoFormViewModel request);
}