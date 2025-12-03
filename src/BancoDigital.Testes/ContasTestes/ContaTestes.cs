using BancoDigital.Dominio.Interfaces;
using BancoDigital.Dominio.Modelos;
using BancoDigital.Servicos.Servicos;
using Moq;

namespace BancoDigital.Testes.ContasTestes
{
    public class ContaTestes
    {
        private readonly Mock<IContaRepository> _contaRepoMock;
        private readonly ContaService _service;

        public ContaTestes()
        {
            _contaRepoMock = new Mock<IContaRepository>();
            _service = new ContaService(_contaRepoMock.Object);
        }

        [Fact]
        public async Task CriarContaAsync_DeveCriarConta_QuandoNumeroNaoExiste()
        {
            _contaRepoMock
                .Setup(r => r.ObterPorNumeroAsync("123", It.IsAny<CancellationToken>()))
                .ReturnsAsync((Conta?)null);

            var conta = await _service.CriarContaAsync("123", 100m);

            Assert.Equal("123", conta.Numero);
            Assert.Equal(100m, conta.Saldo);

            _contaRepoMock.Verify(r => r.AdicionarAsync(It.Is<Conta>(c =>
                c.Numero == "123" && c.Saldo == 100m
            ), It.IsAny<CancellationToken>()), Times.Once);

            _contaRepoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CriarContaAsync_DeveLancarErro_QuandoNumeroJaExiste()
        {
            var contaExistente = new Conta { Numero = "123", Saldo = 50m };
            _contaRepoMock
                .Setup(r => r.ObterPorNumeroAsync("123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(contaExistente);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CriarContaAsync("123", 100m));

            Assert.Equal("Já existe uma conta com esse número.", ex.Message);
            _contaRepoMock.Verify(r => r.AdicionarAsync(It.IsAny<Conta>(), It.IsAny<CancellationToken>()), Times.Never);
        }


        [Fact]
        public async Task DepositarAsync_DeveAumentarSaldo_QuandoValorValido()
        {
            var conta = new Conta { Numero = "123", Saldo = 100m };

            _contaRepoMock
                .Setup(r => r.ObterPorNumeroAsync("123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(conta);

            var saldoFinal = await _service.DepositarAsync("123", 50m);

            Assert.Equal(150m, saldoFinal);
            Assert.Equal(150m, conta.Saldo);

            _contaRepoMock.Verify(r => r.AtualizarAsync(conta, It.IsAny<CancellationToken>()), Times.Once);
            _contaRepoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task DepositarAsync_DeveLancarArgumentException_QuandoValorInvalido(decimal valor)
        {
            var conta = new Conta { Numero = "123", Saldo = 100m };

            _contaRepoMock
                .Setup(r => r.ObterPorNumeroAsync("123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(conta);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.DepositarAsync("123", valor));

            Assert.Equal("Valor de depósito deve ser maior que zero", ex.Message);
            _contaRepoMock.Verify(r => r.AtualizarAsync(It.IsAny<Conta>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DepositarAsync_DeveLancarErro_QuandoContaNaoExiste()
        {
            _contaRepoMock
                .Setup(r => r.ObterPorNumeroAsync("999", It.IsAny<CancellationToken>()))
                .ReturnsAsync((Conta?)null);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.DepositarAsync("999", 10m));

            Assert.Equal("Conta não encontrada", ex.Message);
        }

        [Fact]
        public async Task SacarAsync_DeveDiminuirSaldo_QuandoSaldoSuficiente()
        {
            var conta = new Conta { Numero = "123", Saldo = 100m };

            _contaRepoMock
                .Setup(r => r.ObterPorNumeroAsync("123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(conta);

            var saldoFinal = await _service.SacarAsync("123", 40m);

            Assert.Equal(60m, saldoFinal);
            Assert.Equal(60m, conta.Saldo);

            _contaRepoMock.Verify(r => r.AtualizarAsync(conta, It.IsAny<CancellationToken>()), Times.Once);
            _contaRepoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public async Task SacarAsync_DeveLancarArgumentException_QuandoValorInvalido(decimal valor)
        {
            var conta = new Conta { Numero = "123", Saldo = 100m };

            _contaRepoMock
                .Setup(r => r.ObterPorNumeroAsync("123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(conta);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.SacarAsync("123", valor));

            Assert.Equal("Valor de saque deve ser maior que zero", ex.Message);
            _contaRepoMock.Verify(r => r.AtualizarAsync(It.IsAny<Conta>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task SacarAsync_DeveLancarErro_QuandoSaldoInsuficiente()
        {
            var conta = new Conta { Numero = "123", Saldo = 50m };

            _contaRepoMock
                .Setup(r => r.ObterPorNumeroAsync("123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(conta);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.SacarAsync("123", 100m));

            Assert.Equal("Saldo insuficiente", ex.Message);
            Assert.Equal(50m, conta.Saldo);
        }

        [Fact]
        public async Task SacarAsync_DeveLancarErro_QuandoContaNaoExiste()
        {
            _contaRepoMock
                .Setup(r => r.ObterPorNumeroAsync("999", It.IsAny<CancellationToken>()))
                .ReturnsAsync((Conta?)null);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.SacarAsync("999", 10m));

            Assert.Equal("Conta não encontrada", ex.Message);
        }

        [Fact]
        public async Task ObterSaldoAsync_DeveRetornarSaldo_QuandoContaExiste()
        {
            var conta = new Conta { Numero = "123", Saldo = 80m };

            _contaRepoMock
                .Setup(r => r.ObterPorNumeroAsync("123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(conta);

            var saldo = await _service.ObterSaldoAsync("123");

            Assert.Equal(80m, saldo);
        }

        [Fact]
        public async Task ObterSaldoAsync_DeveLancarErro_QuandoContaNaoExiste()
        {
            _contaRepoMock
                .Setup(r => r.ObterPorNumeroAsync("999", It.IsAny<CancellationToken>()))
                .ReturnsAsync((Conta?)null);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.ObterSaldoAsync("999"));

            Assert.Equal("Conta não encontrada", ex.Message);
        }
    }
}
