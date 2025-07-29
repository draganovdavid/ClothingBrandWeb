using ClothingBrand.Data.Models;
using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static ClothingBrandApp.GCommon.ApplicationConstants;

using static ClothingBrandApp.Web.Infrastructure.Extensions.ServiceCollectionExtensions;

namespace ClothingBrandApp.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
              throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            builder.Services.AddAuthorization();
            builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddRepositories(typeof(IShopRepository).Assembly);
            builder.Services.AddUserDefinedServices(typeof(IShopService).Assembly);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(AllowAllDomainsPolicy, policyBuilder =>
                {
                    policyBuilder
                        .WithOrigins("https://localhost:7180")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapIdentityApi<ApplicationUser>();
            app.MapControllers();

            app.Run();
        }
    }
}
