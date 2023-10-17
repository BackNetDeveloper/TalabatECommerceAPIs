using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace APIsMainProject.Extentions
{
    public static class SwaggerServiceExtentions
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(Config =>
            {
                Config.SwaggerDoc("v1", new OpenApiInfo { Title = "RouteAPIsProject", Version = "v1" });

                var SecurityShema = new OpenApiSecurityScheme 
                {
                    Description = "JWT Auth Bearer Scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    Reference = new OpenApiReference 
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                Config.AddSecurityDefinition("Bearer", SecurityShema);
                var SecurityRequirments = new OpenApiSecurityRequirement
                {
                    {SecurityShema , new[]{ "Bearer"} }
                };
                Config.AddSecurityRequirement(SecurityRequirments);
            });
            return services;
        }
    }
}
