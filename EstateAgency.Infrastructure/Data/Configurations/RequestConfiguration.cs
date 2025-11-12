using EstateAgency.Domain;
using EstateAgency.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EstateAgency.Infrastructure.Data.Configurations;

/// <summary>
/// Конфигурация сущности Request 
/// </summary>
public class RequestConfiguration : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.ToTable("Requests");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .ValueGeneratedOnAdd();
         
        builder.Property(r => r.ClientId)
            .IsRequired()
            .HasColumnName("ClientId");

        builder.Property(r => r.PropertyId)
            .IsRequired()
            .HasColumnName("PropertyId");
         
        var requestTypeConverter = new EnumToStringConverter<RequestType>();
         
        builder.Property(r => r.Type)
            .IsRequired()
            .HasConversion(requestTypeConverter)
            .HasMaxLength(20)
            .HasColumnName("RequestType");

        builder.Property(r => r.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasColumnName("Amount");

        builder.Property(r => r.CreatedDate)
            .IsRequired()
            .HasColumnName("CreatedDate");
         
        builder.HasOne(r => r.Client)
            .WithMany(c => c.Requests)
            .HasForeignKey(r => r.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Property)
            .WithMany(p => p.Requests)
            .HasForeignKey(r => r.PropertyId)
            .OnDelete(DeleteBehavior.Restrict);
         
        builder.HasIndex(r => r.Type)
            .HasDatabaseName("IX_Requests_Type");

        builder.HasIndex(r => r.CreatedDate)
            .HasDatabaseName("IX_Requests_CreatedDate");

        builder.HasIndex(r => new { r.ClientId, r.Type })
            .HasDatabaseName("IX_Requests_ClientId_Type");

        builder.HasIndex(r => new { r.PropertyId, r.Type })
            .HasDatabaseName("IX_Requests_PropertyId_Type");

        builder.HasIndex(r => r.Amount)
            .HasDatabaseName("IX_Requests_Amount");
    }
}