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
    public class TransacaoRepository : ITransacaoRepository
    {
        internal SqlServerContext _context;

        public TransacaoRepository(SqlServerContext context)
        {
            _context = context;
        }

        public async Task<Transacao> Criar(Transacao entity)
        {
            await _context.Transacoes.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Transacao>> Listar(int contaId)
        {
            return await _context.Transacoes
                                 .AsNoTracking()
                                 .Where(x => x.ContaId == contaId)
                                 .OrderByDescending(x => x.Data)
                                 .ToListAsync();
        }

        public decimal Saldo(int contaId)
        {
            return _context.Transacoes
                           .AsNoTracking()
                           .Where(x => x.ContaId == contaId)
                           .Sum(x => x.Valor);
        }
    }
}
