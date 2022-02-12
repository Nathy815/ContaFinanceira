using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Domain.Requests
{
    public class AutenticacaoRequest
    {
        public int ContaId { get; set; }
        public string Senha { get; set; }
    }
}
