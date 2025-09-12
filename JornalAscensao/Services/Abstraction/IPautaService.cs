using System.Linq.Expressions;
using JornalAscensao.Models;

namespace JornalAscensao.Services.Abstraction;

public interface IPautaService
{
    public Task<IEnumerable<PautaViewModel>> GetPautasAsync();
    public Task<PautaViewModel?> GetPautaAsync(Guid id);
    public Task<PautaFormViewModel> CriarPautaAsync(PautaFormViewModel request);
    public Task<PautaFormViewModel?> AtualizarPautaAsync(Guid id,PautaFormViewModel request);
    
    public Task<bool> FecharPautaAsync(Guid id);
    public Task<bool> ExcluirPautaAsync(Guid id);
    public Task<PautaMetadados> GetPautaMetadadosAsync(string request);
    
}