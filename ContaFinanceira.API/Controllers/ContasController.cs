using ContaFinanceira.Domain.Enum;
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
    public class ContasController : ControllerBase
    {
        private readonly IContaService _service;
        private readonly ILogger<ContasController> _logger;

        public ContasController(IContaService service,
                                ILogger<ContasController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Criar(ContaRequest request)
        {
            try
            {
                _logger.LogInformation("Iniciando requisição de Criar() com {request}...", JsonConvert.SerializeObject(request));

                var result = await _service.Criar(request);

                _logger.LogInformation("Requisição Criar() processada com sucesso! Retorno: {result}", JsonConvert.SerializeObject(result));

                return new CreatedResult("", result);
            }
            catch (ValidationException val)
            {
                _logger.LogWarning("Validação da requisição de Criar() retornou erros. Detalhes: {erros}", JsonConvert.SerializeObject(val));

                return new BadRequestObjectResult(val.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao processar requisição Criar(). Detalhes: {erro}", JsonConvert.SerializeObject(ex));

                return StatusCode(500, ex.Message);
            }
        }

        
    }
}
