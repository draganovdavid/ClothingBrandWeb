using ClothingBrand.Data.Seeding.Interfaces;
using ClothingBrandApp.Web.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ClothingBrandApp.Web.Infrastructure.Extensions
{
    public static class WebApplicationExtensions
    {
        public static IApplicationBuilder UseManagerAccessRestriction(this IApplicationBuilder app)
        {
            app.UseMiddleware<ManagerAccessRestrictionMiddleware>();

            return app;
        }

        public static IApplicationBuilder UserAdminRedirection(this IApplicationBuilder app)
        {
            app.UseMiddleware<AdminRedirectionMiddleware>();

            return app;
        }

        public static IApplicationBuilder SeedDefaultIdentity(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            IServiceProvider serviceProvider = scope.ServiceProvider;

            IIdentitySeeder identitySeeder = serviceProvider
                .GetRequiredService<IIdentitySeeder>();
            identitySeeder
                .SeedIdentityAsync()
                .GetAwaiter()
                .GetResult();

            return app;
        }
    }
}