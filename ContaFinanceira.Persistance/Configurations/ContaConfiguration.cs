using ContaFinanceira.Domain.Entities;
using ContaFinanceira.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Persistance.Configurations
{
    public class ContaConfiguration : IEntityTypeConfiguration<Conta>
    {
        public void Configure(EntityTypeBuilder<Conta> builder)
        {
            builder.ToTable("tbcf_contas")
                   .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.DataCriacao)
                   .IsRequired();

            builder.Property(x => x.Senha)
                   .IsRequired();

            builder.HasOne(x => x.Cliente)
                   .WithOne(x => x.Conta)
                   .HasForeignKey<Cliente>(x => x.ContaId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.Id);

            builder.HasData(new Conta()
            {
                Id = 1,
                AgenciaId = 1,
                DataCriacao = Convert.ToDateTime("2022-02-11 23:18:00"),
                Senha = CriptografiaUtil.CriptografarSenha("12345")
            });
        }
    }
}
