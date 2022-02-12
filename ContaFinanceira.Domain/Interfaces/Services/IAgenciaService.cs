using ContaFinanceira.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Domain.Interfaces.Services
{
    public interface IAgenciaService
    {
        Task<List<AgenciaResponse>> Listar();
        Task<bool> ValidaAgencia(int id);
    }
}
