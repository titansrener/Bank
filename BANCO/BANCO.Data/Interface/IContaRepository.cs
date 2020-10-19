using BANCO.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BANCO.Data.Interface
{
    public interface IContaRepository
    {
        Task<List<Conta>> GetContasAsync();
        Task<Conta> GetContaBancoAsync(string conta, string nomeBanco);
        Task<Conta> AtualizarSaldoAsync(Conta conta);
        Task<Conta> CadastrarAsync(Conta conta);
    }
}
