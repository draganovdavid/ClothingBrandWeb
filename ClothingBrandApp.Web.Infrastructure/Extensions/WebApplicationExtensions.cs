using ClothingBrandApp.Web.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace ClothingBrandApp.Web.Infrastructure.Extensions
{
    public static class WebApplicationExtensions
    {
        public static IApplicationBuilder UseManagerAccessRestriction(this IApplicationBuilder app)
        {
            app.UseMiddleware<ManagerAccessRestrictionMiddleware>();

            return app;
        }
    }
}