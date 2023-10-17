using APIsMainProject.Extentions;
using APIsMainProject.Helper;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace APIsMainProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            builder.Services.AddSingleton<IConnectionMultiplexer>(Config => 
            {
                var Configration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("RedisConnection"), true);
                return ConnectionMultiplexer.Connect(Configration);
            });
            builder.Services.GetApplicationServices(); // Extention Method (See => APIsMainProject/Extentions)
            builder.Services.AddIdentityService(builder.Configuration); // Extention Method (See => APIsMainProject/Extentions)
            builder.Services.AddSwaggerService(); // Extention Method (See => APIsMainProject/Extentions)

            builder.Services.AddCors (
                options => 
                {
                     options.AddPolicy("CorsPolicy",Policy => Policy.AllowAnyOrigin()
                                                                    .AllowAnyHeader()
                                                                    .AllowAnyMethod()
                                                                    .SetIsOriginAllowed(origin=>true));
                } );
            #endregion

            var app = builder.Build();

           
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerfactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = services.GetRequiredService<StoreDbContext>();
                    await context.Database.MigrateAsync();
                    // Data Seeding When The First Run Of The Application 
                    await StoreDbContextSeed.SeedAsync(context,loggerfactory);

                    //------------------------------------------------------------------------

                    var usermanger = services.GetRequiredService<UserManager<AppUser>>();
                    var identitycontext = services.GetRequiredService<AppIdentityDbContext>();
                    // Data Seeding When The First Run Of The Application 
                    await identitycontext.Database.MigrateAsync();
                    await AppIdentityDbContextSeed.SeedUserAsync(usermanger);
                }
                catch (Exception ex)
                {
                    var logger = loggerfactory.CreateLogger<Program>();
                    logger.LogError(ex ,"Error Occured While Seeding The Data !!");
                }
            }

            #region Configue pipeline
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(S=>S.SwaggerEndpoint("/swagger/v1/swagger.json", "RouteAPIsProject v1"));
            }
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.MapControllers();

            app.Run(); 
            #endregion
        }
    }
}