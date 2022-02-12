using ContaFinanceira.Domain.Interfaces.Services;
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
    public class AgenciaController : ControllerBase
    {
        private readonly IAgenciaService _service;

        public AgenciaController(IAgenciaService service)
        {
            _service = service;
        }

        [HttpGet]
        public Task<List<AgenciaResponse>> Listar()
        {
            try
            {
                return _service.Listar();
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
