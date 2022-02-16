using ContaFinanceira.Domain.Entities;
using ContaFinanceira.Domain.Interfaces.Repositories;
using ContaFinanceira.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        public async Task<Transacao> Criar(Transacao transacao)
        {
            await _context.Transacoes.AddAsync(transacao);
            await _context.SaveChangesAsync();

            return await _context.Transacoes
                                 .AsNoTracking()
                                 .Include(x => x.Conta)
                                      .ThenInclude(x => x.Cliente)
                                 .Where(x => x.Id == transacao.Id)
                                 .FirstOrDefaultAsync();
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

        public async Task SetNotificacaoEnviada(int id)
        {
            var entity = await _context.Transacoes.Where(x => x.Id == id).FirstOrDefaultAsync();
            entity.NotificacaoEnviada = true;
            
            _context.Transacoes.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
