using StockApp.API.Middleware;
using StockApp.Infra.IoC;
using StockApp.Infra.Data.Identity;
using StockApp.Domain.Interfaces;
using StockApp.Infra.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using StockApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using StockApp.Application.Interfaces;
using StockApp.Application.Services;
internal class Program
{
    private static void Main(string[] args)
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

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IMfaService, MfaService>();



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
                ValidIssuer = builder.Configuration["JwtSeettigs:Issuer "],
                ValidAudience = builder.Configuration["JwtSeettigs:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("CanManageProducts", policy =>
            policy.Requirements.Add( new 
            ClaimsAuthorizationRequirement("Permission", "CanManageProducts")));
        });
        builder.Services.AddSingleton<IAuthorizationHandler,
            ClaimsAuthorizationHandler>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseErrorHandlerMiddleware();
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}