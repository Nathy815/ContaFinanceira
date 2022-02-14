using ContaFinanceira.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Domain.Interfaces.Repositories
{
    public interface IContaRepository
    {
        Task<Conta> Criar(Conta conta);
        Task<Conta> Pesquisar(int id);
        Task<Conta> PesquisarPorEmailCliente(string email);
    }
}
