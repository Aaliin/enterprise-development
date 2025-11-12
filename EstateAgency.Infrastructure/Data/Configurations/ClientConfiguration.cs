using EstateAgency.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EstateAgency.Infrastructure.Data.Configurations;

/// <summary>
/// Конфигурация сущности Client 
/// </summary>
public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd(); 

        builder.Property(c => c.FullName)
            .IsRequired() 
            .HasMaxLength(100)
            .HasColumnName("FullName"); 

        builder.Property(c => c.PassportNumber)
            .IsRequired()
            .HasMaxLength(20) 
            .HasColumnName("PassportNumber");

        builder.Property(c => c.PhoneNumber)
            .IsRequired() 
            .HasMaxLength(20) 
            .HasColumnName("PhoneNumber");
         
        builder.HasMany(c => c.Requests)  
            .WithOne(r => r.Client)  
            .HasForeignKey(r => r.ClientId)  
            .OnDelete(DeleteBehavior.Restrict); 

        builder.HasIndex(c => c.FullName)
            .HasDatabaseName("IX_Clients_FullName");

        builder.HasIndex(c => c.PassportNumber)
            .IsUnique() 
            .HasDatabaseName("IX_Clients_PassportNumber");
    }
}