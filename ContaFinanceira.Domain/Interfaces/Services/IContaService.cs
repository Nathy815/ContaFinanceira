using ContaFinanceira.Domain.Requests;
using ContaFinanceira.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Domain.Interfaces.Services
{
    public interface IContaService
    {
        Task<ContaResponse> Criar(ContaRequest request);
        Task<bool> ValidaContaExiste(int id);
        Task<bool> ValidaSenhaCorreta(int contaId, string senha);
        Task<bool> ValidaEmailJaExiste(string email);
    }
}
