var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("SqlServer")
    .AddDatabase("EstateAgencyDb");

builder.AddProject<Projects.EstateAgency_Api>("estateagency-api")
    .WithReference(sql)
    .WithEnvironment("UseEf", "true")
    .WaitFor(sql);

builder.Build().Run();