using ContaFinanceira.Domain.Interfaces.Services;
using ContaFinanceira.Domain.Requests;
using ContaFinanceira.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContaFinanceira.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaController : ControllerBase
    {
        private readonly IContaService _service;

        public ContaController(IContaService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ContaResponse> Criar(ContaRequest request)
        {
            try
            {
                return await _service.Criar(request);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
