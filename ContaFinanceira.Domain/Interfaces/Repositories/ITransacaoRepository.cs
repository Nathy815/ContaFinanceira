using ContaFinanceira.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Domain.Interfaces.Repositories
{
    public interface ITransacaoRepository : IBaseRepository<Transacao>
    {
        decimal Saldo(int contaId);
        Task<List<Transacao>> Listar(int contaId);
    }
}