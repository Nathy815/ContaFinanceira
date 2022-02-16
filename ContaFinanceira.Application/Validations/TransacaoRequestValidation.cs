using ContaFinanceira.Domain.Interfaces.Services;
using ContaFinanceira.Domain.Requests;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContaFinanceira.Application.Validations
{
    public class TransacaoRequestValidation : AbstractValidator<TransacaoRequest>
    {
        private readonly ITransacaoService _transacaoService;
        private readonly IHttpContextAccessor _httpContext;
            
        public TransacaoRequestValidation(ITransacaoService transacaoService,
                                          IHttpContextAccessor httpContext)
        {
            _transacaoService = transacaoService;
            _httpContext = httpContext;

            var claim = _httpContext.HttpContext.User.Claims.FirstOrDefault();
            var contaId = Convert.ToInt32(claim.Value);

            RuleFor(x => x.Valor)
                .NotEqual(0)
                    .WithMessage("Por favor, informe um valor.")
                .Must((all, el) => _transacaoService.ValidarSaldoSuficiente(contaId, el))
                    .WithMessage("Saldo insuficiente.");
        }
    }
}