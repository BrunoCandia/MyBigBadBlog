using MyBigBadBlog.Common;

var builder = DistributedApplication.CreateBuilder(args);

#region Redis Cache

var cache = builder.AddRedis(Constants.OUTPUTCACHE) ////optional port 65028
    .WithRedisCommander();

#endregion

#region Postgres Database

var dbPassword = builder.AddParameter("DatabasePassword", secret: true);

var dbServer = builder.AddPostgres(Constants.DATABASESERVERNAME, password: dbPassword);
var db = dbServer.AddDatabase(Constants.DATABASENAME);

dbServer.WithDataVolume()
        .WithPgAdmin();

#endregion

#region Web Site

builder.AddProject<Projects.MyBigBadBlog_Web>(Constants.WEB)
    .WithReference(cache)
    .WithReference(db)
    .WithExternalHttpEndpoints();

#endregion

#region Worker Service

builder.AddProject<Projects.MyBigBadBlog_Service_DatabaseMigration>(Constants.DATABASEMIGRATION)
    .WithReference(db);

#endregion

await builder.Build().RunAsync();
