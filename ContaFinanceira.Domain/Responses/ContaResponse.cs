using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Domain.Responses
{
    public class ContaResponse
    {
        public int Id { get; set; }
        public int AgenciaId { get; set; }
        public string ClienteNome { get; set; }
    }
}
