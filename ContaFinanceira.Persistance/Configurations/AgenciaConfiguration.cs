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
    public class AgenciaConfiguration : IEntityTypeConfiguration<Agencia>
    {
        public void Configure(EntityTypeBuilder<Agencia> builder)
        {
            builder.ToTable("tbcf_agencias")
                   .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.Nome)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.HasData(new List<Agencia>() 
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

        }
    }
}
