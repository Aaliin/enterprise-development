var builder = DistributedApplication.CreateBuilder(args);

builder.AddContainer("Nats", "nats")
    .WithArgs("-js")
    .WithHttpEndpoint(8222, targetPort: 8222)
    .WithEndpoint(4222, targetPort: 4222, name: "client");

builder.AddProject<Projects.EstateAgency_Api>("Api")
    .WithEnvironment("AppMode", "Lab4")
    .WithEnvironment("UseEf", "true")
    .WithEnvironment("UseNats", "true")
    .WithEnvironment("ConnectionStrings__DefaultConnection",
        "Server=(localdb)\\mssqllocaldb;Database=EstateAgencyDb;Trusted_Connection=True;TrustServerCertificate=true;")
    .WithEnvironment("Nats__Url", "nats://localhost:4222");

builder.AddProject<Projects.EstateAgency_ContractGenerator>("Generator")
    .WithEnvironment("AppMode", "Lab4")
    .WithEnvironment("Nats__Url", "nats://localhost:4222");

builder.Build().Run();