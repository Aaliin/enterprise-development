using EstateAgency.Domain;
using EstateAgency.Domain.Data;
using EstateAgency.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace EstateAgency.Infrastructure.Data;

/// <summary> 
/// Содержит конфигурацию модели данных и логику заполнения начальными данными
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Коллекция клиентов риэлторского агентства
    /// </summary>
    public DbSet<Client> Clients { get; set; }

    /// <summary>
    /// Коллекция объектов недвижимости
    /// </summary>
    public DbSet<Property> Properties { get; set; }

    /// <summary>
    /// Коллекция заявок от клиентов
    /// </summary>
    public DbSet<Request> Requests { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр контекста базы данных
    /// </summary>
    /// <param name="options">Опции конфигурации DbContext</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    /// <summary>
    /// Настраивает модель данных при создании контекста 
    /// </summary>
    /// <param name="modelBuilder">Построитель модели данных</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
         
        modelBuilder.ApplyConfiguration(new ClientConfiguration());
        modelBuilder.ApplyConfiguration(new PropertyConfiguration());
        modelBuilder.ApplyConfiguration(new RequestConfiguration());
         
        ConfigureRelationships(modelBuilder);
    }

    /// <summary>
    /// Настраивает отношения между сущностями базы данных 
    /// </summary>
    /// <param name="modelBuilder">Построитель модели данных</param>
    private static void ConfigureRelationships(ModelBuilder modelBuilder)
    { 
        modelBuilder.Entity<Request>()
            .HasOne(r => r.Client)
            .WithMany(c => c.Requests)
            .HasForeignKey(r => r.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
         
        modelBuilder.Entity<Request>()
            .HasOne(r => r.Property)
            .WithMany(p => p.Requests)
            .HasForeignKey(r => r.PropertyId)
            .OnDelete(DeleteBehavior.Restrict);
         
        modelBuilder.Entity<Client>()
            .HasIndex(c => c.FullName)
            .HasDatabaseName("IX_Clients_FullName");

        modelBuilder.Entity<Property>()
            .HasIndex(p => p.Type)
            .HasDatabaseName("IX_Properties_Type");

        modelBuilder.Entity<Property>()
            .HasIndex(p => p.Address)
            .HasDatabaseName("IX_Properties_Address");

        modelBuilder.Entity<Request>()
            .HasIndex(r => r.Type)
            .HasDatabaseName("IX_Requests_Type");

        modelBuilder.Entity<Request>()
            .HasIndex(r => r.CreatedDate)
            .HasDatabaseName("IX_Requests_CreatedDate");

        modelBuilder.Entity<Request>()
            .HasIndex(r => new { r.ClientId, r.Type })
            .HasDatabaseName("IX_Requests_ClientId_Type");

        modelBuilder.Entity<Request>()
            .HasIndex(r => new { r.PropertyId, r.Type })
            .HasDatabaseName("IX_Requests_PropertyId_Type");
    }

    /// <summary>
    /// Заполняет базу данных тестовыми данными 
    /// </summary>
    public void SeedData()
    {
        if (!Clients.Any())
        {
            var (clients, properties, requests) = DataSeeder.GetCompleteTestData();

            Clients.AddRange(clients);
            Properties.AddRange(properties);
            Requests.AddRange(requests);

            SaveChanges();
        }
    }
}