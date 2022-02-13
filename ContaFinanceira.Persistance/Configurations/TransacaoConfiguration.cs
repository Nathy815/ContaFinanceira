using ContaFinanceira.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Persistance.Configurations
{
    public class TransacaoConfiguration : IEntityTypeConfiguration<Transacao>
    {
        public void Configure(EntityTypeBuilder<Transacao> builder)
        {
            builder.ToTable("tbcf_transacoes")
                   .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.Data)
                   .IsRequired();

            builder.Property(x => x.Valor)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.HasOne(x => x.Conta)
                   .WithMany(x => x.Transacoes)
                   .HasForeignKey(x => x.ContaId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.Id, x.Data, x.Valor });

            builder.HasData(new Transacao() 
            { 
                ContaId = 1,
                Id = 1,
                Data = Convert.ToDateTime("2022-02-11 23:18:00"),
                Valor = 10M
            });
        }
    }
}
