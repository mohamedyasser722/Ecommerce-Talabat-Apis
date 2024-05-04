using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Middlewares;
using Talabat.Repository.Data;
using Talabat.APIs.Helpers.Extensions;
using StackExchange.Redis;
using Talabat.Repository.Identity;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region Configure services (Dependency Injection)
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Services.AddControllers(); //Register Required Web APIs services to the DI Container

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"));
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddApplicationServices();

            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddSwaggerServices();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", options =>
                {
                    options.WithOrigins(builder.Configuration["ClientUrl"])
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowAnyOrigin();
                });
            });
            
            //AppBuilder
            var app = builder.Build();
            #endregion

            #region Update-Database
            //StoreContext dbContext = new StoreContext();
            //await dbContext.Database.MigrateAsync(); //Update Database
            using var scope = app.Services.CreateScope(); //All Scoped Services
            var services = scope.ServiceProvider; //DI
                                                  //LoggerFactory
            var dbContext = services.GetRequiredService<StoreContext>(); //Ask Clr to create Object From DbContext Explicitly 
            var IdentityDbContext = services.GetRequiredService<AppIdentityDbContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var LoggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await dbContext.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(dbContext); //This will Seed every Run
                await IdentityDbContext.Database.MigrateAsync();
                await AppIdentityDbContextSeed.SeedUserAsync(userManager);
            }
            catch (Exception ex)
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "An Error Occurred during Migrations applying");
            }

            #endregion

            #region Configure MiddleWares
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
                app.UseSwaggerMiddleWares();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseCors("MyPolicy");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStatusCodePagesWithRedirects("/errors/{0}");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}