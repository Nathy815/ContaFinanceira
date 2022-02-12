using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Domain.Entities
{
    public class Transacao : Base
    {
        public int ContaId { get; set; }
        public virtual Conta Conta { get; set; }

        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
    }
}
