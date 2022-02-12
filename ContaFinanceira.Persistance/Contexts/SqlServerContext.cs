using ContaFinanceira.Domain.Entities;
using ContaFinanceira.Persistance.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ContaFinanceira.Persistance.Contexts
{
    public class SqlServerContext : DbContext
    {
        public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options) { }
        public SqlServerContext() { }

        public virtual DbSet<Agencia> Agencias { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Conta> Contas { get; set; }
        public virtual DbSet<Transacao> Transacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AgenciaConfiguration());
            modelBuilder.ApplyConfiguration(new ContaConfiguration());
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            modelBuilder.ApplyConfiguration(new TransacaoConfiguration());
        }
    }
}
