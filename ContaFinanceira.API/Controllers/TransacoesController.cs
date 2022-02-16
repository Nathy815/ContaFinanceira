using ContaFinanceira.Domain.Interfaces.Services;
using ContaFinanceira.Domain.Requests;
using ContaFinanceira.Domain.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ContaFinanceira.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransacoesController : ControllerBase
    {
        private readonly ITransacaoService _service;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<TransacoesController> _logger;

        public TransacoesController(ITransacaoService service,
                                    IHttpContextAccessor httpContext,
                                    ILogger<TransacoesController> logger)
        {
            _service = service;
            _httpContext = httpContext;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Adicionar(TransacaoRequest request)
        {
            try
            {
                _logger.LogInformation("Verificando cliente logado para requisição Adicionar()...");

                var contaId = _httpContext.HttpContext.User.Claims.FirstOrDefault().Value;
                request.setConta(Convert.ToInt32(contaId));

                _logger.LogInformation("Iniciando requisição de Adicionar() com {request}...", JsonConvert.SerializeObject(request));

                var result = await _service.Adicionar(request);

                _logger.LogInformation("Requisição Adicionar() processada com sucesso! Retorno: {result}", JsonConvert.SerializeObject(result));

                return new CreatedResult("", result);
            }
            catch(ValidationException val)
            {
                _logger.LogWarning("Validação da requisição de Adicionar() retornou erros. Detalhes: {erros}", JsonConvert.SerializeObject(val));

                return new BadRequestObjectResult(val.Message);
            }
            catch(Exception ex)
            {
                _logger.LogCritical("Erro ao processar requisição Adicionar(). Detalhes: {erro}", JsonConvert.SerializeObject(ex));

                return StatusCode(500, ex.Message);
            }
        }
    }
}