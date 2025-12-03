namespace BancoDigital.Infraestrutura.Repositorios;

public class ContaRepository : IContaRepository
{
    private readonly BancoDigitalDbContext _context;

    public ContaRepository(BancoDigitalDbContext context)
    {
        _context = context;
    }

    public async Task<Conta?> ObterPorNumeroAsync(string numero, CancellationToken ct = default)
    {
        return await _context.Contas
            .FirstOrDefaultAsync(c => c.Numero == numero, ct);
    }

    public async Task AdicionarAsync(Conta conta, CancellationToken ct = default)
    {
        await _context.Contas.AddAsync(conta, ct);
    }

    public Task AtualizarAsync(Conta conta, CancellationToken ct = default)
    {
        _context.Contas.Update(conta);
        return Task.CompletedTask;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);
}
