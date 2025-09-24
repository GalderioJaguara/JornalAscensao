using JornalAscensao.Models;

namespace JornalAscensao.Services.Abstraction;

public interface IHomeService
{
    public Task<HomeViewModel> GetHomeViewModelAsync();
}