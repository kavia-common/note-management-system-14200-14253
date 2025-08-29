using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NotesAppBackend.Data;
using NotesAppBackend.Repositories;
using NotesAppBackend.Services;
using NotesAppBackend.Settings;

namespace NotesAppBackend.Extensions
{
    /// <summary>
    /// Extension helpers for DI and middleware setup.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppDb(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
        {
            // For simplicity use InMemory provider by default for development.
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseInMemoryDatabase("NotesAppDb");
            });

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();
            return services;
        }

        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INoteService, NoteService>();
            return services;
        }

        public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration config)
        {
            var section = config.GetSection("Jwt");
            services.Configure<JwtSettings>(section);
            var jwt = section.Get<JwtSettings>() ?? new JwtSettings();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwt.Issuer,
                        ValidAudience = jwt.Audience,
                        IssuerSigningKey = key
                    };
                });

            return services;
        }

        public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Notes App API",
                    Version = "v1",
                    Description = "REST API for Notes management with JWT authentication."
                });

                var jwtScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer {token}'",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", jwtScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtScheme, new List<string>() }
                });
            });

            return services;
        }
    }
}
