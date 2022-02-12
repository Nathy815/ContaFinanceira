using ContaFinanceira.Domain.Interfaces.Repositories;
using ContaFinanceira.Domain.Interfaces.Services;
using ContaFinanceira.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Application.Services
{
    public class AgenciaService : IAgenciaService
    {
        private readonly IAgenciaRepository _agenciaRepository;

        public AgenciaService(IAgenciaRepository agenciaRepository)
        {
            _agenciaRepository = agenciaRepository;
        }

        public async Task<List<AgenciaResponse>> Listar()
        {
            var dbAgencias = await _agenciaRepository.Listar();

            var agencias = new List<AgenciaResponse>();
            foreach (var agencia in dbAgencias)
                agencias.Add(new AgenciaResponse() { Id = agencia.Id, Nome = agencia.Nome });

            return agencias;
        }

        public async Task<bool> ValidaAgencia(int id)
        {
            return await _agenciaRepository.Pesquisar(id) != null;
        }
    }
}
