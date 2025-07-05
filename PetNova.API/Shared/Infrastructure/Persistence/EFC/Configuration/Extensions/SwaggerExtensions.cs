using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class SwaggerExtensions
{
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        // Configuración consolidada
        return services.AddEndpointsApiExplorer().AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo 
            { 
                Title = "PetNova Veterinary API", 
                Version = "v1",
                Description = "API for veterinary clinic management",
                Contact = new OpenApiContact
                {
                    Name = "PetNova Support",
                    Email = "support@petnova.com"
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License"
                }
            });
            // Configuración explícita del esquema OpenAPI
            c.UseOneOfForPolymorphism();
            c.SelectDiscriminatorNameUsing(baseType => "$type");
            c.SelectDiscriminatorValueUsing(subType => subType.Name);
            // Configuración de seguridad JWT
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            // Incluir comentarios XML
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }

            c.EnableAnnotations();
        });
    }

    public static IApplicationBuilder UseCustomSwaggerUI(this IApplicationBuilder app)
    {
        return app
            .UseSwagger()
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetNova Veterinary API v1");
                c.RoutePrefix = "swagger";
                c.DisplayRequestDuration();
                c.EnableDeepLinking();
                c.DefaultModelsExpandDepth(-1);
                c.DisplayOperationId();
                c.EnableFilter();
            });
    }

}