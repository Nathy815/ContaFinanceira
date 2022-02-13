using ContaFinanceira.Domain.Interfaces.Services;
using ContaFinanceira.Domain.Requests;
using ContaFinanceira.Domain.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public TransacoesController(ITransacaoService service,
                                    IHttpContextAccessor httpContext)
        {
            _service = service;
            _httpContext = httpContext;
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
                var contaId = _httpContext.HttpContext.User.Claims.FirstOrDefault();

                if (contaId == null)
                    return StatusCode(401, "Usuário não logado.");

                request.setConta(Convert.ToInt32(contaId.Value));

                var result = await _service.Adicionar(request);

                return new CreatedResult("", result);
            }
            catch(ValidationException val)
            {
                return new BadRequestObjectResult(val.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}