using ContaFinanceira.Domain.Entities;
using ContaFinanceira.Domain.Interfaces.Repositories;
using ContaFinanceira.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Persistance.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        internal SqlServerContext _context;

        public ClienteRepository(SqlServerContext context)
        {
            _context = context;
        }

        public async Task<Cliente> Criar(Cliente entity)
        {
            await _context.Clientes.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Cliente> PesquisarPorConta(int contaId)
        {
            return await _context.Clientes
                                 .AsNoTracking()
                                 .Include(x => x.Conta)
                                    .ThenInclude(x => x.Transacoes)
                                 .Where(x => x.Conta.Id == contaId)
                                 .FirstOrDefaultAsync();
        }
    }
}
