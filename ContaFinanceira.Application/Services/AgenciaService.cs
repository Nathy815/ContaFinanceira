using ContaFinanceira.Domain.Interfaces.Repositories;
using ContaFinanceira.Domain.Interfaces.Services;
using ContaFinanceira.Domain.Responses;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        private readonly ILogger<AgenciaService> _logger;

        public AgenciaService(IAgenciaRepository agenciaRepository,
                              ILogger<AgenciaService> logger)
        {
            _agenciaRepository = agenciaRepository;
            _logger = logger;
        }

        public async Task<List<AgenciaResponse>> Listar()
        {
            _logger.LogInformation("Buscando agências no banco de dados...");

            var dbAgencias = await _agenciaRepository.Listar();

            _logger.LogInformation("Banco de dados retornou {lista}", JsonConvert.SerializeObject(dbAgencias));
            _logger.LogInformation("Convertendo dados do banco em List<AgenciaResponse>...");

            var agencias = new List<AgenciaResponse>();
            foreach (var agencia in dbAgencias)
                agencias.Add(new AgenciaResponse() { Id = agencia.Id, Nome = agencia.Nome });

            _logger.LogInformation("Método Listar() retornando {retorno}", JsonConvert.SerializeObject(agencias));

            return agencias;
        }

        public async Task<bool> ValidaAgencia(int id)
        {
            _logger.LogInformation("Validando se agência Id:{id} existe no banco de dados...", id);

            return await _agenciaRepository.Pesquisar(id) != null;
        }
    }
}
