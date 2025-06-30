using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StockApp.API.GraphQL; 
using StockApp.API.Middleware;
using StockApp.Application.Interfaces;
using StockApp.Application.Services;
using StockApp.Domain.Entities;
using StockApp.Domain.Interfaces;
using StockApp.Infra.Data.Identity;
using StockApp.Infra.Data.Repositories;
using StockApp.API.Hubs;
using StockApp.Infra.IoC;
using System.Text;
using StockApp.Infra.Data.Identity.Authorization;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpClient<IErpIntegrationService, ErpIntegrationService>();

        // Add services to the container.
        builder.Services.AddInfrastructureAPI(builder.Configuration);

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("Redis");
            options.InstanceName = "StockApp";
        });

        builder.Services.AddControllers();
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("CanManageStock", policy => policy.Requirements.Add(new PermissionRequirement("CanManageStock")));
        });
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IInventoryService, InventoryService>();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "StockApp.API", Version = "v1" });

            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };
            c.AddSecurityDefinition("Bearer", securitySchema);
            var securityRequeriment = new OpenApiSecurityRequirement
        {
            { securitySchema, new[] { "Bearer" } }
        };
            c.AddSecurityRequirement(securityRequeriment);
        }

        );

        builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
        builder.Services.AddScoped<IMfaService, MfaService>();
        builder.Services.AddSignalR();
        builder.Services.AddScoped<IAuditService, AuditService>();

        var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSeettigs:SecretKey"]);
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JwtSeettigs:Issuer"],
                ValidAudience = builder.Configuration["JwtSeettigs:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("CanManageProducts", policy =>
            policy.Requirements.Add(new
            ClaimsAuthorizationRequirement("Permission", "CanManageProducts")));

        });
        builder.Services.AddSingleton<IAuthorizationHandler, ClaimsAuthorizationHandler>();

        // Configuração do GraphQL
        builder.Services.AddGraphQLServer()
            .AddQueryType<Query>()
            .AddMutationType<Mutation>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseErrorHandlerMiddleware();

        app.UseCors("AllowAll");

        app.MapHub<StockHub>("/stockhub");
        app.UseHttpsRedirection();


        app.UseAuthorization();

        app.MapGraphQL();
        app.MapControllers();

        app.Run();
    }
}