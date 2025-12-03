namespace BancoDigital.Infraestrutura.BancoDeDados;

public class BancoDigitalDbContext : DbContext
{
    public DbSet<Conta> Contas => Set<Conta>();
    public BancoDigitalDbContext(DbContextOptions<BancoDigitalDbContext> options): base(options)
    {
    }
}
