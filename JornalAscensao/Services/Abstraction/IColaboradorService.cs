using JornalAscensao.Models;

namespace JornalAscensao.Services.Abstraction;

public interface IColaboradorService
{
    public Task<ColaboradorViewModel> GetColaboradorDataAsync();

}