using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Domain.Entities
{
    public class Agencia : Base
    {
        public string Nome { get; set; }

        public virtual ICollection<Conta> Contas { get; set; }
    }
}
