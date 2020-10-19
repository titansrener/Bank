using BANCO.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BANCO.Data.Configuration
{
    public class ContaConfiguration : IEntityTypeConfiguration<Conta>
    {
        public void Configure(EntityTypeBuilder<Conta> builder)
        {
            builder.ToTable("TB_CONTAS", "dbo");

            builder.HasKey(p => p.Id)
                .HasName("PK_TB_CONTA");

            builder.Property(e => e.NumeroConta)
                .HasColumnName("NUMERO_CONTA")
                .HasColumnType("varchar(9)");

            builder.Property(e => e.Saldo)
                .HasColumnName("SALDO")
                .HasColumnType("decimal(18, 2)");

            builder.Property(e => e.CpfCliente)
                .HasColumnName("CPF_CLIENTE")
                .HasMaxLength(11)
                .HasColumnType("varchar(11)");

            builder.Property(e => e.NomeCliente)
                .HasColumnName("NOME_CLIENTE")
                .HasMaxLength(60)
                .HasColumnType("varchar(60)");

            builder.Property(e => e.RendaMensal)
                .HasColumnName("RENDA_MENSAL_CLIENTE")
                .HasColumnType("decimal(18, 2)");

            builder.Property(e => e.NomeBanco)
                .HasColumnName("NOME_BANCO")
                .HasMaxLength(40)
                .HasColumnType("varchar(40)");

            builder.Property(e => e.DataDeposito)
                .HasColumnName("DT_DEPOSITO")
                .HasColumnType("Datetime");
        }
    }
}
