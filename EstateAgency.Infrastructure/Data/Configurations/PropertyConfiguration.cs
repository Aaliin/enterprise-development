using EstateAgency.Domain;
using EstateAgency.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EstateAgency.Infrastructure.Data.Configurations;

/// <summary>
/// Конфигурация сущности Property 
/// </summary>
public class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.ToTable("Properties");
         
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();
         
        var propertyTypeConverter = new EnumToStringConverter<PropertyType>();
        var propertyPurposeConverter = new EnumToStringConverter<PropertyPurpose>();
         
        builder.Property(p => p.Type)
            .IsRequired()
            .HasConversion(propertyTypeConverter)
            .HasMaxLength(20)
            .HasColumnName("PropertyType");

        builder.Property(p => p.Purpose)
            .IsRequired()
            .HasConversion(propertyPurposeConverter)
            .HasMaxLength(20)
            .HasColumnName("PropertyPurpose");

        builder.Property(p => p.CadastralNumber)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("CadastralNumber");

        builder.Property(p => p.Address)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("Address");

        builder.Property(p => p.Floors)
            .IsRequired()
            .HasColumnName("Floors");

        builder.Property(p => p.TotalArea)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasColumnName("TotalArea");

        builder.Property(p => p.Rooms)
            .IsRequired()
            .HasColumnName("Rooms");

        builder.Property(p => p.CeilingHeight)
            .HasColumnType("decimal(5,2)")
            .HasColumnName("CeilingHeight");

        builder.Property(p => p.Floor)
            .HasColumnName("Floor");

        builder.Property(p => p.HasEncumbrances)
            .IsRequired()
            .HasColumnName("HasEncumbrances");

        builder.HasMany(p => p.Requests)
            .WithOne(r => r.Property)
            .HasForeignKey(r => r.PropertyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => p.Type)
            .HasDatabaseName("IX_Properties_Type");

        builder.HasIndex(p => p.CadastralNumber)
            .IsUnique()
            .HasDatabaseName("IX_Properties_CadastralNumber");

        builder.HasIndex(p => p.Address)
            .HasDatabaseName("IX_Properties_Address");
    }
}