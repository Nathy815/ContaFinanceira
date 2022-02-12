using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Domain.Requests
{
    public class ContaRequest
    {
        public string NomeCliente { get; set; }
        public int AgenciaId { get; set; }
        public string Senha { get; set; }
        public decimal? DepositoInicial { get; set; }
    }
}
