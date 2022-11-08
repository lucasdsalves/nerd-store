using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NSE.Client.API.Models;
using NSE.Core.DomainObjects;

namespace NSE.Client.API.Data.Mappings
{
    public class ClientsMapping : IEntityTypeConfiguration<Clients>
    {
        public void Configure(EntityTypeBuilder<Clients> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.OwnsOne(c => c.Cpf, tf =>
            {
                tf.Property(c => c.Number)
                    .IsRequired()
                    .HasMaxLength(Cpf.CpfMaxLength)
                    .HasColumnName("Cpf")
                    .HasColumnType($"varchar({Cpf.CpfMaxLength})");
            });

            builder.OwnsOne(c => c.Email, tf =>
            {
                tf.Property(c => c.EmailAddress)
                    .IsRequired()
                    .HasColumnName("Email")
                    .HasColumnType($"varchar({Email.EmailAddressMaxLength})");
            });

            // 1 : 1 => Client : Address
            builder.HasOne(c => c.Address)
                .WithOne(c => c.Client);

            builder.ToTable("Clients");
        }
    }
}