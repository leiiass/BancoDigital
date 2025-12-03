namespace BancoDigital.Api.GraphQL
{
    public class Query
    {
        public async Task<decimal> Saldo(int conta, [Service] ContaService contaService)
        {
            var numeroConta = conta.ToString();
            var saldo = await contaService.ObterSaldoAsync(numeroConta);
            return saldo;
        }
    }
}
