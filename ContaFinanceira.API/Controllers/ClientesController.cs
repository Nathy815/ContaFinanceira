using ContaFinanceira.Domain.Interfaces.Services;
using ContaFinanceira.Domain.Requests;
using ContaFinanceira.Domain.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace ContaFinanceira.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(IClienteService clienteService,
                                  ILogger<ClientesController> logger)
        {
            _clienteService = clienteService;
            _logger = logger;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Autenticar(AutenticacaoRequest request)
        {
            try
            {
                _logger.LogInformation("Iniciando requisição de Autenticar() com {request}...", JsonConvert.SerializeObject(request));

                var result = await _clienteService.Autenticar(request);

                _logger.LogInformation("Requisição Autenticar() processada com sucesso! Retorno: {result}", JsonConvert.SerializeObject(result));

                return new OkObjectResult(result);
            }
            catch (ValidationException val)
            {
                _logger.LogWarning("Validação da requisição de Autenticar() retornou erros. Detalhes: {erros}", JsonConvert.SerializeObject(val));

                return new BadRequestObjectResult(val.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao processar requisição Autenticar(). Detalhes: {erro}", JsonConvert.SerializeObject(ex));

                return StatusCode(500, ex.Message);
            }
        }
    }
}
