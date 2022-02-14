using ContaFinanceira.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace ContaFinanceira.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgenciasController : ControllerBase
    {
        private readonly IAgenciaService _service;
        private readonly ILogger<AgenciasController> _logger;

        public AgenciasController(IAgenciaService service,
                                  ILogger<AgenciasController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Listar()
        {
            try
            {
                _logger.LogInformation("Iniciando requisição de Listar()...");

                var result = await _service.Listar();

                _logger.LogInformation("Requisição Listar() processada com sucesso! Retorno: {result}", JsonConvert.SerializeObject(result));

                return new OkObjectResult(result);
            }
            catch(Exception ex)
            {
                _logger.LogError("Erro ao processar requisição Listar(). Detalhes: {erro}", JsonConvert.SerializeObject(ex));

                return StatusCode(500, ex.Message);
            }
        }
    }
}
