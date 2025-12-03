namespace BancoDigital.Dominio.Modelos
{
    public class Conta
    {
        public int Id { get; set; }
        public string Numero { get; set; } = null!;
        public decimal Saldo { get; set; }
    }
}
