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

        public async Task<Conta> Criar(Conta entity)
        {
            await _context.Contas.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Conta> Pesquisar(int id)
        {
            return await _context.Contas
                                 .AsNoTracking()
                                 .Where(x => x.Id == id)
                                 .FirstOrDefaultAsync();
        }
    }
}
