using BancoDigital.Dominio.Interfaces;
using BancoDigital.Dominio.Modelos;

namespace BancoDigital.Servicos.Servicos
{
    public class ContaService
    {
        private const string MsgValorDepositoInvalido = "Valor de depósito deve ser maior que zero";
        private const string MsgValorSaqueInvalido = "Valor de saque deve ser maior que zero";
        private const string MsgSaldoInsuficiente = "Saldo insuficiente";
        private const string MsgContaNaoEncontrada = "Conta não encontrada";
        private const string MsgContaJaExiste = "Já existe uma conta com esse número.";

        private readonly IContaRepository _contaRepository;

        public ContaService(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<decimal> DepositarAsync(string numeroConta, decimal valor, CancellationToken ct = default)
        {
            var conta = await ObterContaOuErro(numeroConta, ct);

            if (valor <= decimal.Zero)
                throw new ArgumentException(MsgValorDepositoInvalido);

            conta.Saldo += valor;

            await _contaRepository.AtualizarAsync(conta, ct);
            await _contaRepository.SaveChangesAsync(ct);

            return conta.Saldo;
        }

        public async Task<decimal> SacarAsync(string numeroConta, decimal valor, CancellationToken ct = default)
        {
            var conta = await ObterContaOuErro(numeroConta, ct);

            if (valor <= decimal.Zero)
                throw new ArgumentException(MsgValorSaqueInvalido);

            if (valor > conta.Saldo)
                throw new InvalidOperationException(MsgSaldoInsuficiente);

            conta.Saldo -= valor;

            await _contaRepository.AtualizarAsync(conta, ct);
            await _contaRepository.SaveChangesAsync(ct);

            return conta.Saldo;
        }

        public async Task<decimal> ObterSaldoAsync(string numeroConta, CancellationToken ct = default)
        {
            var conta = await ObterContaOuErro(numeroConta, ct);
            return conta.Saldo;
        }

        private async Task<Conta> ObterContaOuErro(string numeroConta, CancellationToken ct)
        {
            var conta = await _contaRepository.ObterPorNumeroAsync(numeroConta, ct);

            return conta is null ? throw new InvalidOperationException(MsgContaNaoEncontrada) : conta;
        }

        public async Task<Conta> CriarContaAsync(string numeroConta, decimal saldoInicial = decimal.Zero, CancellationToken ct = default)
        {
            var existente = await _contaRepository.ObterPorNumeroAsync(numeroConta, ct);
            if (existente is not null)
                throw new InvalidOperationException(MsgContaJaExiste);

            var conta = new Conta
            {
                Numero = numeroConta,
                Saldo = saldoInicial
            };

            await _contaRepository.AdicionarAsync(conta, ct);
            await _contaRepository.SaveChangesAsync(ct);

            return conta;
        }
    }
}
