using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var database = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin()
    .AddDatabase("webhooks");

var queue = builder.AddRabbitMQ("rabbitmq")
    .WithDataVolume()
    .WithManagementPlugin();

builder.AddProject<Projects.Webhook_Api>("webhook-api")
    .WithReference(database)
    .WithReference(queue)
    .WaitFor(queue)
    .WaitFor(database);

builder.AddProject<Projects.Webhook_Processing>("webhook-processing")
    .WithReplicas(3)
    .WithReference(database)
    .WithReference(queue)
    .WaitFor(queue)
    .WaitFor(database);

builder.Build().Run();
