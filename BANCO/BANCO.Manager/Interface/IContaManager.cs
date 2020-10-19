using BANCO.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BANCO.Manager.Interface
{
    public interface IContaManager
    {
        Task<List<Conta>> GetContasAsync();
        Task<Conta> GetContaBancoAsync(string conta, string nomeBanco);
        Task<Conta> SacarAsync(string conta, string nomeBanco, decimal valorSaque);
        Task<Conta> DepositarAsync(string conta, string nomeBanco, decimal valorDeposito);
        Task<Conta> CadastrarAsync(Conta conta);
        Task<Conta> TransferirAsync(string numeroContaOrigem, string nomeBancoOrigem, string numeroContaDestino, string nomeBancoDestino, decimal valorTransferir);
    }
}
