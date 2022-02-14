using ContaFinanceira.Domain.Entities;
using ContaFinanceira.Domain.Interfaces.Repositories;
using ContaFinanceira.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ContaFinanceira.Persistance.Repositories
{
    public class ContaRepository : IContaRepository
    {
        internal SqlServerContext _context;

        public ContaRepository(SqlServerContext context)
        {
            _context = context;
        }

        public async Task<Conta> Criar(Conta conta)
        {
            await _context.Contas.AddAsync(conta);
            await _context.SaveChangesAsync();

            return conta;
        }

        public async Task<Conta> Pesquisar(int id)
        {
            return await _context.Contas
                                 .AsNoTracking()
                                 .Where(x => x.Id == id)
                                 .FirstOrDefaultAsync();
        }

        public async Task<Conta> PesquisarPorEmailCliente(string email)
        {
            return await _context.Contas
                                 .AsNoTracking()
                                 .Include(x => x.Cliente)
                                 .Where(x => x.Cliente.Email.Equals(email))
                                 .FirstOrDefaultAsync();
        }
    }
}
