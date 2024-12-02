using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyBigBadBlog.Common;

namespace MyBigBadBlog.Data.Postgres
{
    public static class Extensions
    {
        public static IHostApplicationBuilder AddPostgresFeatures(this IHostApplicationBuilder hostApplicationBuilder)
        {
            hostApplicationBuilder.AddNpgsqlDbContext<ApplicationDbContext>(Constants.DATABASENAME);

            hostApplicationBuilder.Services.AddScoped<IPostRepository, PgRepository>();

            return hostApplicationBuilder;
        }

        public static IHostApplicationBuilder AddIdentity(this IHostApplicationBuilder hostApplicationBuilder)
        {
            hostApplicationBuilder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                                           .AddEntityFrameworkStores<ApplicationDbContext>();

            return hostApplicationBuilder;
        }
    }
}
