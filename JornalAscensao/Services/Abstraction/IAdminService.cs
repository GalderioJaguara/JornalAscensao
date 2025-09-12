namespace JornalAscensao.Services.Abstraction;

public interface IAdminService
{
    public Task<int> GetPautasStatusAync(bool status = false);
    public Task<int> GetArtigosPublicadosAsync();
    public Task<int> GetArtigosRevisaoAsync();
    public Task<int> GetNovosColaboradoresAsync();
    public Task<int> GetColaboradoresBloqueadosAsync();
}