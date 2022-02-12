using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Domain.Requests
{
    public class TransacaoRequest
    {
        public int ContaId { get; private set; }

        public decimal Valor { get; set; }

        public void setConta(int contaId)
        {
            ContaId = contaId;
        }
    }
}
