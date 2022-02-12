using ContaFinanceira.Domain.Interfaces.Services;
using ContaFinanceira.Domain.Requests;
using ContaFinanceira.Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ContaFinanceira.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransacaoController : ControllerBase
    {
        private readonly ITransacaoService _service;

        public TransacaoController(ITransacaoService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize]
        public async Task<List<TransacaoResponse>> Adicionar(TransacaoRequest request)
        {
            try
            {
                var contaId = HttpContext.User.Claims.FirstOrDefault();
                request.setConta(Convert.ToInt32(contaId.Value));

                return await _service.Adicionar(request);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}