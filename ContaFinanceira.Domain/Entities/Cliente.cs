using ContaFinanceira.Domain.Enum;
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
        public ePessoa TipoPessoa { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
       
        public int ContaId { get; set; }
        public virtual Conta Conta { get; set; }
    }
}
