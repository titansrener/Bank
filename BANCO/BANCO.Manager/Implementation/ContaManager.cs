using BANCO.Core;
using BANCO.Core.Exceptions;
using BANCO.Data.Interface;
using BANCO.Manager.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BANCO.Manager.Implementation
{
    public class ContaManager : IContaManager
    {
        private readonly IContaRepository _contaRepository;

        public ContaManager(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<Conta> CadastrarAsync(Conta conta)
        {
            if (conta.NomeBanco.ToUpper() == "A")
            {
                if (conta.RendaMensal < 5000)
                {
                    return await _contaRepository.CadastrarAsync(conta);
                }
                else
                {
                    throw new BusinessException("Renda mensal deve ser menor que R$5000,00.");
                }

            }

            if (conta.NomeBanco.ToUpper() == "C")
            {
                if (conta.RendaMensal > 20000)
                {
                    return await _contaRepository.CadastrarAsync(conta);
                }
                else
                {
                    throw new BusinessException("Renda mensal deve sermaior que R$20000,00.");
                }
            }
            return await _contaRepository.CadastrarAsync(conta);
        }

        public async Task<Conta> GetContaBancoAsync(string conta, string nomeBanco)
        {
            return await _contaRepository.GetContaBancoAsync(conta, nomeBanco);
        }

        public async Task<List<Conta>> GetContasAsync()
        {
            return await _contaRepository.GetContasAsync();
        }

        private decimal CalcularDescontos(string nomeBanco, decimal valor)
        {
            decimal descontoTransacao = 0;
            if (nomeBanco == "A")
                descontoTransacao = (10 * (decimal)0.03);
            else if (nomeBanco == "B")
                descontoTransacao = (10 * (decimal)0.012);
            else if (nomeBanco == "C")
                descontoTransacao = (10 * (decimal)0.08);
            return descontoTransacao;
        }

        public async Task<Conta> SacarAsync(string conta, string nomeBanco, decimal valorSaque)
        {
            decimal descontoTransacao = CalcularDescontos(nomeBanco, valorSaque);

            if (nomeBanco.ToUpper() != "B")
            {
                if (valorSaque <= 500)
                {
                    var cc = await _contaRepository.GetContaBancoAsync(conta, nomeBanco);
                    if (cc == null)
                        throw new BusinessException("Banco ou conta incorretos.");
                    else
                    {
                        if (valorSaque <= cc.Saldo)
                        {
                            cc.Saldo -= valorSaque - descontoTransacao;
                            return await _contaRepository.AtualizarSaldoAsync(cc);
                        }
                        else
                            throw new BusinessException("Saldo insuficiente.");
                    }
                }
                else
                {
                    throw new BusinessException("Não é possível sacar mais que R$500,00.");
                }
            }
            else
            {
                throw new BusinessException("Não é possível realizar saques no banco: " + nomeBanco);
            }
        }

        public async Task<Conta> DepositarAsync(string conta, string nomeBanco, decimal valorDeposito)
        {
            decimal descontoTransacao = CalcularDescontos(nomeBanco, valorDeposito);

            var cc = await _contaRepository.GetContaBancoAsync(conta, nomeBanco);
            if (cc == null)
                throw new BusinessException("Banco ou conta incorretos.");
            else
            {
                if (cc.DataDeposito == null)
                {
                    if (valorDeposito <= 700)
                    {
                        cc.Saldo += valorDeposito - descontoTransacao;
                        cc.DataDeposito = DateTime.Now;
                        return await _contaRepository.AtualizarSaldoAsync(cc);
                    }
                    else
                    {
                        throw new BusinessException("Não é possível depositar mais que R$700,00.");
                    }
                }
                else if (cc.DataDeposito.Value.DayOfYear == DateTime.Now.DayOfYear
                    && cc.DataDeposito.Value.Hour != DateTime.Now.Hour && valorDeposito <= 700)
                {
                    cc.Saldo += valorDeposito - descontoTransacao;
                    cc.DataDeposito = DateTime.Now;
                    return await _contaRepository.AtualizarSaldoAsync(cc);
                }
                else
                {
                    throw new BusinessException("Não é possível depositar mais que R$700,00 por hora.");
                }
            }
        }

        public async Task<Conta> TransferirAsync(string numeroContaOrigem, string nomeBancoOrigem, string numeroContaDestino, string nomeBancoDestino, decimal valorTransferir)
        {
            var co = await _contaRepository.GetContaBancoAsync(numeroContaOrigem, nomeBancoOrigem);
            if (co == null)
                throw new BusinessException("Banco ou conta de origem incorretos.");

            var cd = await _contaRepository.GetContaBancoAsync(numeroContaDestino, nomeBancoDestino);
            if (cd == null)
                throw new BusinessException("Banco ou conta de destino incorretos.");

            Conta contaOrigem = await SacarAsync(co.NumeroConta, co.NomeBanco, valorTransferir);
            if (contaOrigem != null)
            {
                if (await DepositarAsync(cd.NumeroConta, cd.NomeBanco, valorTransferir) != null)
                {
                    return contaOrigem;
                }
                else
                {
                    await DepositarAsync(co.NumeroConta, co.NomeBanco, valorTransferir);
                    throw new BusinessException("A transferencia não foi concluída.");
                }
            }
            return null;
        }
    }
}
