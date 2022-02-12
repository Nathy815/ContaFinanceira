using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Domain.Entities
{
    public class Conta : Base
    {
        public DateTime DataCriacao { get; set; }
        public string Senha { get; set; }

        public int AgenciaId { get; set; }
        public virtual Agencia Agencia { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual ICollection<Transacao> Transacoes { get; set; }
    }
}
