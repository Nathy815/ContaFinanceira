using ContaFinanceira.Domain.Entities;
using ContaFinanceira.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Domain.Interfaces.Services
{
    public interface IEmailService
    {
        Task EnviarNotificacao(Transacao transacao);
    }
}
