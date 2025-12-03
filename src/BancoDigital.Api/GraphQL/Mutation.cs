namespace BancoDigital.Api.GraphQL
{
    public class Mutation
    {
        public async Task<SaldoResult> Depositar(string conta, decimal valor,[Service] ContaService contaService)
        {
            var saldo = await contaService.DepositarAsync(conta, valor);
            return new SaldoResult(conta, saldo);
        }

        public async Task<SaldoResult> Sacar(string conta, decimal valor,[Service] ContaService contaService)
        {
            var saldo = await contaService.SacarAsync(conta, valor);
            return new SaldoResult(conta, saldo);
        }

        public async Task<SaldoResult> CriarConta(string conta, decimal saldoInicial, [Service] ContaService contaService)
        {
            var novaConta = await contaService.CriarContaAsync(conta, saldoInicial);
            return new SaldoResult(novaConta.Numero, novaConta.Saldo);
        }
    }
}
