namespace JornalAscensao.Models;

public abstract class BaseModel
{
    public Guid Id { get; set; }
    public DateTime Criado { get; set; } = DateTime.UtcNow;
    public DateTime Atualizado { get; set; } = DateTime.UtcNow;
    public DateTime? Excluido { get; set; }

    public void Atualizar()
    {
        Atualizado = DateTime.UtcNow;
    }
}