using EstateAgency.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EstateAgency.EF.Data;

/// <summary>
/// Определяет модели данных и их конфигурацию для работы с базой данных
/// </summary>
public class EstateAgencyDbContext : DbContext
{
    /// <summary>
    /// Инициализирует новый экземпляр контекста с указанными параметрами
    /// </summary>
    /// <param name="options">Параметры настройки контекста базы данных</param>
    public EstateAgencyDbContext(DbContextOptions<EstateAgencyDbContext> options) : base(options) { }

    /// <summary>
    /// Инициализирует новый экземпляр контекста без параметров
    /// </summary>
    public EstateAgencyDbContext() { }

    /// <summary>
    /// Набор данных клиентов агентства недвижимости
    /// </summary>
    public DbSet<Client> Clients { get; set; }

    /// <summary>
    /// Набор данных объектов недвижимости
    /// </summary>
    public DbSet<Property> Properties { get; set; }

    /// <summary>
    /// Набор данных заявок на операции с недвижимостью
    /// </summary>
    public DbSet<Request> Requests { get; set; }

    /// <summary>
    /// Настраивает параметры подключения к базе данных
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EstateAgencyDB;Trusted_Connection=true;TrustServerCertificate=true;");
        }
    }

    /// <summary>
    /// Настраивает модели данных и их отношения в базе данных
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Конфигурация сущности Client
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PassportNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);

            entity.HasIndex(e => e.PassportNumber).IsUnique();

            entity.HasMany(c => c.Requests)
                .WithOne(r => r.Client)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Конфигурация сущности Property
        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CadastralNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Address).IsRequired().HasMaxLength(500);
            entity.Property(e => e.TotalArea).HasColumnType("decimal(18,2)");
            entity.Property(e => e.CeilingHeight).HasColumnType("decimal(5,2)");

            entity.HasIndex(e => e.CadastralNumber).IsUnique();

            entity.Property(e => e.Type)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(e => e.Purpose)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.HasMany(p => p.Requests)
                .WithOne(r => r.Property)
                .HasForeignKey(r => r.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Конфигурация сущности Request
        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2");

            entity.Property(e => e.Type)
                .HasConversion<string>()
                .HasMaxLength(10);
        });
    }
}