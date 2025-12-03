using BancoDigital.Dominio.Modelos;

namespace BancoDigital.Dominio.Interfaces
{
    public interface IContaRepository
    {
        Task<Conta?> ObterPorNumeroAsync(string numero, CancellationToken ct = default);
        Task AdicionarAsync(Conta conta, CancellationToken ct = default);
        Task AtualizarAsync(Conta conta, CancellationToken ct = default);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
