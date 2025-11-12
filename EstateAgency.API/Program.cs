using EstateAgency.Application;
using EstateAgency.Infrastructure;
using EstateAgency.Infrastructure.Data;

namespace EstateAgency.API;

/// <summary>
/// Точка входа приложения 
/// </summary>
public class Program
{
    /// <summary>
    /// Основная точка входа приложения
    /// </summary>
    /// <param name="args">Аргументы командной строки</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);
        var app = builder.Build();
        ConfigureMiddleware(app);
        app.Run();
    }

    /// <summary>
    /// Определяет порядок выполнения
    /// </summary>
    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "EstateAgency API",
                Version = "v1",
                Description = "API for Estate Agency Management"
            });
        });

        builder.Services.AddApplicationServices(); 
        builder.Services.AddInMemoryInfrastructure();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        builder.Services.AddScoped<DatabaseInitializer>();
    }

    /// <summary>
    /// Настраивает pipeline middleware приложения
    /// </summary>
    /// <param name="app">Экземпляр приложения</param>
    private static void ConfigureMiddleware(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
            initializer.Initialize();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "EstateAgency API v1");
                options.RoutePrefix = "swagger";
            });
        }

        app.UseCors("AllowAll");

        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();
    }
}

/// <summary>
/// Инициализатор базы данных
/// </summary>
public class DatabaseInitializer(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;

    /// <summary>
    /// Выполняет инициализацию базы данных тестовыми данными
    /// </summary>
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        _context.SeedData();
    }
}