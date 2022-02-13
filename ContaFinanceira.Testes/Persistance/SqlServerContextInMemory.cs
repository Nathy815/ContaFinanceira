using ContaFinanceira.Domain.Entities;
using ContaFinanceira.Domain.Interfaces.Repositories;
using ContaFinanceira.Persistance.Contexts;
using ContaFinanceira.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Testes.Persistance
{
    public class SqlServerContextInMemory
    {
        protected readonly SqlServerContext _context;

        public SqlServerContextInMemory()
        {
            var options = new DbContextOptions<SqlServerContext>();
            var builder = new DbContextOptionsBuilder<SqlServerContext>();
            
            builder.UseInMemoryDatabase("contafinanceira");
            options = builder.Options;

            _context = new SqlServerContext(options);
            _context.Database.EnsureCreated();
            _context.Database.EnsureDeleted();
        }

        public IAgenciaRepository GetAgenciaRepository()
        {
            _context.Agencias.AddRange(new List<Agencia>()
            {
                new Agencia()
                {
                    Id = 1,
                    Nome = "Agência 1"
                },
                new Agencia()
                {
                    Id = 2,
                    Nome = "Agência 2"
                }
            });

            _context.SaveChanges();

            return new AgenciaRepository(_context);
        }

        public IClienteRepository GetClienteRepository()
        {
            _context.Clientes.AddRange(new List<Cliente>()
            {
                new Cliente()
                {
                    Id = 1,
                    Nome = "Cliente 1",
                    CpfCnpj = "12345678901",
                    TipoPessoa = Domain.Enum.ePessoa.PessoaFisica,
                    ContaId = 1,
                    Conta = new Conta() 
                    {
                        Id = 1,
                        AgenciaId = 1,
                        DataCriacao = DateTime.Now,
                        Senha = "12345"
                    }
                },
                new Cliente()
                {
                    Id = 2,
                    Nome = "Cliente 2",
                    CpfCnpj = "12345678901234",
                    TipoPessoa = Domain.Enum.ePessoa.PessoaJuridica,
                    ContaId = 2,
                    Conta = new Conta()
                    {
                        Id = 2,
                        AgenciaId = 1,
                        DataCriacao = DateTime.Now,
                        Senha = "12345"
                    }
                }
            });

            _context.SaveChanges();

            return new ClienteRepository(_context);
        }

        public IContaRepository GetContaRepository()
        {
            _context.Contas.AddRange(new List<Conta>()
            {
                new Conta()
                {
                    Id = 1,
                    AgenciaId = 1,
                    DataCriacao = DateTime.Now,
                    Senha = "12345"
                },
                new Conta()
                {
                    Id = 2,
                    AgenciaId = 1,
                    DataCriacao = DateTime.Now,
                    Senha = "12345"
                }
            });

            _context.SaveChanges();

            return new ContaRepository(_context);
        }

        public ITransacaoRepository GetTransacaoRepository()
        {
            _context.Transacoes.AddRange(new List<Transacao>()
            { 
                new Transacao()
                {
                    Id = 1,
                    Data = DateTime.Now,
                    Valor = 10M,
                    ContaId = 1
                },
                new Transacao()
                {
                    Id = 2,
                    Data = DateTime.Now,
                    Valor = 20M,
                    ContaId = 2
                },
                new Transacao()
                {
                    Id = 3,
                    Data = DateTime.Now,
                    Valor = -5M,
                    ContaId = 1
                },
                new Transacao()
                {
                    Id = 4,
                    Data = DateTime.Now,
                    Valor = -10M,
                    ContaId = 2
                }
            });

            _context.SaveChanges();

            return new TransacaoRepository(_context);
        }
    }
}
