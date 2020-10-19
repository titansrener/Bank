using BANCO.Core;
using BANCO.Data.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BANCO.Data.Implementation
{
    public class ContaRepository : IContaRepository
    {
        private readonly Contexto _context;

        public ContaRepository(Contexto contex)
        {
            _context = contex;
        }
        public async Task<List<Conta>> GetContasAsync()
        {
            return await _context.Contas.ToListAsync();
        }

        public async Task<Conta> GetContaBancoAsync(string conta, string nomeBanco)
        {
            return await _context.Contas.Where(c => c.NumeroConta == conta && c.NomeBanco == nomeBanco).FirstOrDefaultAsync();
        }

        public async Task<Conta> AtualizarSaldoAsync(Conta conta)
        {
            _context.Entry(conta).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return conta;
        }

        public async Task<Conta> CadastrarAsync(Conta conta)
        {
            _context.Entry(conta).State = EntityState.Added;
            await _context.SaveChangesAsync();
            return conta;
        }
    }
}
