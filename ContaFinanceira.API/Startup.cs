using ContaFinanceira.Application.Services;
using ContaFinanceira.Domain.Interfaces.Repositories;
using ContaFinanceira.Domain.Interfaces.Services;
using ContaFinanceira.Persistance.Contexts;
using ContaFinanceira.Persistance.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using FluentValidation.AspNetCore;
using FluentValidation;
using ContaFinanceira.Domain.Requests;
using ContaFinanceira.Application.Validations;
using ContaFinanceira.Middleware;

namespace ContaFinanceira.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            #region Dependency Injection

            services.AddTransient<IAgenciaService, AgenciaService>()
                    .AddTransient<IClienteService, ClienteService>()
                    .AddTransient<IContaService, ContaService>()
                    .AddTransient<ITransacaoService, TransacaoService>();

            services.AddTransient<IAgenciaRepository, AgenciaRepository>()
                    .AddTransient<IClienteRepository, ClienteRepository>()
                    .AddTransient<IContaRepository, ContaRepository>()
                    .AddTransient<ITransacaoRepository, TransacaoRepository>();

            #endregion

            #region Validações

            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0) 
                    .AddFluentValidation();

            services.AddTransient<IValidator<AutenticacaoRequest>, AutenticacaoRequestValidation>()
                    .AddTransient<IValidator<ContaRequest>, ContaRequestValidation>()
                    .AddTransient<IValidator<TransacaoRequest>, TransacaoRequestValidation>();

            #endregion

            #region Database

            services.AddDbContext<SqlServerContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")));

            #endregion

            #region Authentication

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(Configuration.GetSection("Authentication:SecurityKey").Value))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"Invalid Token {context.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine($"Valid token {context.SecurityToken}");
                            return Task.CompletedTask;
                        }
                    };
                });

            #endregion

            #region Authorization

            services.AddAuthorization(options =>
            {
                var defaultPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
                defaultPolicyBuilder = defaultPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultPolicyBuilder.Build();
            });

            #endregion

            services.AddControllers();

            services.AddCors(x =>
            {
                x.AddPolicy("Conta Financeira Policy",
                            builder =>
                            {
                                builder.AllowAnyMethod()
                                       .AllowAnyHeader()
                                       .AllowAnyOrigin();
                            });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Conta Financeira", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
                { 
                    Description = "JWT Bearer Authentication",
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
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Conta Financeira"));
            }

            app.UseMiddleware<LoggerMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors(x => x.AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowAnyOrigin())
               .UseCors("Conta Financeira Policy");
        }
    }
}
