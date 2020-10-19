namespace BANCO.Core.ModelView
{
    public class ContaView
    {
        public string Numero { get; set; }
        public decimal Saldo { get; set; }

        //DADOS CLIENTE
        public string CpfCliente { get; set; }
        public string NomeCliente { get; set; }
        public decimal RendaMensal { get; set; }

        //DADOS BANCO
        public string NomeBanco { get; set; }
    }
}
