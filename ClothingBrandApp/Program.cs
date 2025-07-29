using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ClothingBrand.Data.Models;
using ClothingBrand.Data.Seeding.Interfaces;
using ClothingBrand.Data.Seeding;
using static ClothingBrandApp.Web.Infrastructure.Extensions.WebApplicationExtensions;
using static ClothingBrandApp.Web.Infrastructure.Extensions.ServiceCollectionExtensions;

namespace ClothingBrandApp.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services
                .AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    ConfigureIdentity(builder.Configuration, options);
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddRepositories(typeof(IShopRepository).Assembly);
            builder.Services.AddUserDefinedServices(typeof(IShopService).Assembly);

            // TODO: Implement as extension method
            builder.Services.AddTransient<IIdentitySeeder, IdentitySeeder>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.SeedDefaultIdentity();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseManagerAccessRestriction();

            app.UserAdminRedirection();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }

        private static void ConfigureIdentity(IConfigurationManager configurationManager, IdentityOptions identityOptions)
        {
            identityOptions.SignIn.RequireConfirmedEmail =
                configurationManager.GetValue<bool>($"IdentityConfig:SignIn:RequireConfirmedEmail");
            identityOptions.SignIn.RequireConfirmedAccount =
                configurationManager.GetValue<bool>($"IdentityConfig:SignIn:RequireConfirmedAccount");
            identityOptions.SignIn.RequireConfirmedPhoneNumber =
                configurationManager.GetValue<bool>($"IdentityConfig:SignIn:RequireConfirmedPhoneNumber");

            identityOptions.Password.RequiredLength =
                configurationManager.GetValue<int>($"IdentityConfig:Password:RequiredLength");
            identityOptions.Password.RequireNonAlphanumeric =
                configurationManager.GetValue<bool>($"IdentityConfig:Password:RequireNonAlphanumeric");
            identityOptions.Password.RequireDigit =
                configurationManager.GetValue<bool>($"IdentityConfig:Password:RequireDigit");
            identityOptions.Password.RequireLowercase =
                configurationManager.GetValue<bool>($"IdentityConfig:Password:RequireLowercase");
            identityOptions.Password.RequireUppercase =
                configurationManager.GetValue<bool>($"IdentityConfig:Password:RequireUppercase");
            identityOptions.Password.RequiredUniqueChars =
                configurationManager.GetValue<int>($"IdentityConfig:Password:RequiredUniqueChars");
        }
    }
}
