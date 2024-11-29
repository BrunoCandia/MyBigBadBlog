using MyBigBadBlog.Data.Postgres;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddRedisOutputCache("outputcache");
////builder.AddRedisOutputCache("outputcache",
////    settings =>
////{
////    settings.ConnectionString = "localhost:6379";
////},
////    options =>
////{
////    options.AbortOnConnectFail = false;
////});

builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(policy => policy.Tag("ALL").Expire(TimeSpan.FromMinutes(5)));
    options.AddPolicy("Home", policy => policy.Tag("Home").Expire(TimeSpan.FromSeconds(30)));
    options.AddPolicy("Post", policy => policy.Tag("Post").SetVaryByRouteValue("id").Expire(TimeSpan.FromSeconds(30)));
    ////options.AddPolicy("Post", policy => policy.Tag("Post").SetVaryByRouteValue("slug").Expire(TimeSpan.FromSeconds(30)));
});

// Add services to the container.
////var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
////builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

////builder.AddNpgsqlDbContext<ApplicationDbContext>("MyBigBadBlog");

builder.AddPostgresFeatures();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.AddIdentity();

////builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseOutputCache();

app.MapRazorPages();

// Add new posts to the database
////var pgRepository = app.Services.CreateScope().ServiceProvider.GetRequiredService<MyBigBadBlog.Common.IPostRepository>();
////var pgPsots = new List<(MyBigBadBlog.Common.PostMetadata, string)>
////{
////    (new MyBigBadBlog.Common.PostMetadata("","This is my first post","Jhon Doe",DateTime.UtcNow), "This is the content of my first post. It's a very interesting post, I promise."),
////    (new MyBigBadBlog.Common.PostMetadata("","I think I'll order some pizza","Jhon Doe",DateTime.UtcNow), "There's this really cool new pizza shop that opened down the street called Blazing Pizza. I think I'll give them a try")
////};

////foreach (var post in pgPsots)
////{
////    await pgRepository.AddPostAsync(post.Item1, post.Item2);
////}

await app.RunAsync();
