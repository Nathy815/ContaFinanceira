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
    }
}
