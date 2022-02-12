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
    public class AutenticacaoRequestValidation : AbstractValidator<AutenticacaoRequest>
    {
        private readonly IContaService _contaService;

        public AutenticacaoRequestValidation(IContaService contaService)
        {
            _contaService = contaService;

            RuleFor(x => x.ContaId)
                .NotEqual(0)
                    .WithMessage("Por favor, informe a conta.")
                .Must((all, el) => _contaService.ValidaContaExiste(el).Result)
                    .WithMessage("Conta inválida.");

            RuleFor(x => x.Senha)
                .NotEmpty()
                    .WithMessage("Por favor, informe a senha.")
                .Must((all, el) => _contaService.ValidaSenhaCorreta(all.ContaId, el).Result)
                    .WithMessage("Senha inválida.");
        }
    }
}
