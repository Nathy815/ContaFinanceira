using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Domain.Requests
{
    public class EmailRequest
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public EmailClienteRequest Cliente { get; set; }
        public EmailContaRequest Conta { get; set; }
    }

    public class EmailClienteRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
    }

    public class EmailContaRequest
    {
        public int Id { get; set; }
        public decimal Saldo { get; set; }
    }
}
