var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("SqlServer")
    .AddDatabase("EstateAgencyDb");

builder.AddProject<Projects.EstateAgency_API>("estateagency-api")
    .WithReference(sql)
    .WithEnvironment("UseEf", "true")
    .WaitFor(sql);

builder.Build().Run();