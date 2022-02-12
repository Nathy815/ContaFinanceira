using ContaFinanceira.Domain.Entities;
using ContaFinanceira.Domain.Interfaces.Repositories;
using ContaFinanceira.Persistance.Contexts;
using System;
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

        public async Task<Conta> Criar(Conta entity)
        {
            await _context.Contas.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
