using ContaFinanceira.Domain.Enum;
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
    public class ContaRequestValidation : AbstractValidator<ContaRequest>
    {
        private readonly IAgenciaService _agenciaService;

        public ContaRequestValidation(IAgenciaService agenciaService)
        {
            _agenciaService = agenciaService;

            RuleFor(x => x.NomeCliente)
                .NotEmpty()
                    .WithMessage("Por favor, informe o nome do cliente.")
                .MaximumLength(50)
                    .WithMessage("O nome do cliente não pode ser maior que 50 caracteres.");

            RuleFor(x => x.AgenciaId)
                .NotEqual(0)
                    .WithMessage("Por favor, informe uma agência.")
                .Must((all, el) => _agenciaService.ValidaAgencia(el).Result)
                    .WithMessage("Agência inválida.");

            RuleFor(x => x.TipoPessoa)
                .IsInEnum()
                    .WithMessage("Por favor, informe um valor válido de tipo de pessoa.");

            RuleFor(x => x.CpfCnpj)
                .NotEmpty()
                    .WithMessage("Por favor, informe um CPF/CNPJ.")
                .Length(11)
                    .When(x => x.TipoPessoa == ePessoa.PessoaFisica)
                    .WithMessage("O valor de CPF deve conter 11 caracteres.")
                .Length(14)
                    .When(x => x.TipoPessoa == ePessoa.PessoaJuridica)
                    .WithMessage("O valor de CNPJ deve conter 14 caracteres.");

            RuleFor(x => x.Senha)
                .NotEmpty()
                    .WithMessage("Por favor, informe uma senha.")
                .MinimumLength(5)
                    .WithMessage("A senha não pode ser menor que 5 caracteres.")
                .MaximumLength(10)
                    .WithMessage("A senha não pode ser maior que 10 caracteres.");

            RuleFor(x => x.DepositoInicial)
                .GreaterThan(0)
                    .When(x => x.DepositoInicial.HasValue)
                    .WithMessage("Você não pode sacar na abertura de conta.");
        }
    }
}
