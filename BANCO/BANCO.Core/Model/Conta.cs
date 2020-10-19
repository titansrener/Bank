using BANCO.Core.ModelView;
using System;
using System.ComponentModel.DataAnnotations;

namespace BANCO.Core
{
    public class Conta
    {
        public Conta(ContaView conta)
        {
            NumeroConta = conta.Numero;
            Saldo = conta.Saldo;
            CpfCliente = conta.CpfCliente;
            NomeCliente = conta.NomeCliente;
            RendaMensal = conta.RendaMensal;
            NomeBanco = conta.NomeBanco;
        }
        public Conta()
        {

        }
        //DADOS CONTA
        public int Id { get; set; }
        public string NumeroConta { get; set; }
        public decimal Saldo { get; set; }
        public DateTime? DataDeposito { get; set; }

        //DADOS CLIENTE
        public string CpfCliente { get; set; }
        public string NomeCliente { get; set; }
        public decimal RendaMensal { get; set; }

        //DADOS BANCO
        public string NomeBanco { get; set; }
    }
}
