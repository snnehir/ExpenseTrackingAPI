// using Mapster;

using ExpenseTrackingApp.Infrastructure.Repositories.ExpenseRepository;
using ExpenseTrackingApp.Infrastructure.Repositories.HelperModels;
using ExpenseTrackingApp.Services.Services.ExpenseService;

namespace ExpenseTrackingApp.WebAPI.Extensions
{
    public static class ServiceColllectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, EFUserRepository>();
			services.AddScoped<IExpenseService, ExpenseService>();
			services.AddScoped<IExpenseRepository, EFExpenseRepository>();
			services.AddScoped<IEmailService, EmailService>();
			return services;
        }
        public static IServiceCollection AddAndConfigureSwagger(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            return services;
        }
        public static IServiceCollection AddAndConfigureAuthentication(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(options =>
            {
                options.Audience = configuration.GetSection("Token:Audience").Value;
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = configuration.GetSection("Token:Audience").Value,
                    ValidIssuer = configuration.GetSection("Token:Issuer").Value,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration.GetSection("Token:SecurityKey").Value)),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
            return services;
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("redis");
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString;
                options.InstanceName = "SampleInstance";
            });
            return services;
        }
        public static IServiceCollection AddDbContextService(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("db");

            services.AddDbContext<ExpenseTrackingAppDbContext>(options =>
            {
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("ExpenseTrackingApp.WebAPI"));
            });
            return services;
        }
        public static IServiceCollection AddSessionService(this IServiceCollection services)
        {
            services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromMinutes(15);
            });
            return services;
        }
        public static void MapsterConfigurations(this IServiceCollection services)
        {
			TypeAdapterConfig<DailyExpenseData, DailyExpenseDto>
				.NewConfig().Map(dest => dest.Day, src => src.Date.ToString("M"));

			TypeAdapterConfig<WeeklyExpenseData, WeeklyExpenseDto>
				.NewConfig().Map(dest => dest.Week, src => src.Date);

			TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
        }
    }
}
