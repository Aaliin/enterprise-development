var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
    .AddDatabase("estateagencydb");

builder.AddProject<Projects.EstateAgency_API>("estateagency-api")
    .WithReference(sql)
    .WithEnvironment("UseEF", "true");

builder.Build().Run();