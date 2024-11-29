var builder = DistributedApplication.CreateBuilder(args);

#region Redis Cache

var cache = builder.AddRedis("outputcache", 65028)
    .WithRedisCommander();

#endregion

#region Postgres Database

var dbPassword = builder.AddParameter("DatabasePassword", secret: true);

var dbServer = builder.AddPostgres("dbServer", password: dbPassword);
var db = dbServer.AddDatabase("MyBigBadBlog");

dbServer.WithDataVolume()
        .WithPgAdmin();

#endregion

#region Web Site

builder.AddProject<Projects.MyBigBadBlog_Web>("mybigbadblog-web")
    .WithReference(cache)
    .WithReference(db)
    .WithExternalHttpEndpoints();

#endregion

#region Worker Service

builder.AddProject<Projects.MyBigBadBlog_Service_DatabaseMigration>("mybigbadblog-service-databasemigration")
    .WithReference(db);

#endregion

await builder.Build().RunAsync();
