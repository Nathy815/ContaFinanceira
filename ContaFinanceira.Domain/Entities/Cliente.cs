using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Domain.Entities
{
    public class Cliente : Base
    {
        public string Nome { get; set; }
       
        public int ContaId { get; set; }
        public virtual Conta Conta { get; set; }
    }
}
