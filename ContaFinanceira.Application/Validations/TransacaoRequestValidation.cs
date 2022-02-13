using ContaFinanceira.Domain.Interfaces.Services;
using ContaFinanceira.Domain.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Application.Validations
{
    public class TransacaoRequestValidation : AbstractValidator<TransacaoRequest>
    {
        private readonly ITransacaoService _transacaoService;
        private readonly IContaService _contaService;
            
        public TransacaoRequestValidation(ITransacaoService transacaoService,
                                          IContaService contaService)
        {
            _contaService = contaService;
            _transacaoService = transacaoService;

            RuleFor(x => x.Valor)
                .NotEqual(0)
                    .WithMessage("Por favor, informe um valor.")
                .Must((all, el) => _transacaoService.ValidarSaldoSuficiente(all.ContaId, el))
                    .WithMessage("Saldo insuficiente.");
        }
    }
}