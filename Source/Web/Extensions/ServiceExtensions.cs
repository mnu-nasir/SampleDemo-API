using Contracts;
using Entities.Entities;
using LoggerService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContexts;
using Persistence.Repositories;
using Persistence.Resolvers;
using Service.Contracts;
using Services;
using Web.CustomFormatters;
using System.Threading.RateLimiting;
using Asp.Versioning;

namespace Web.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Configure CORS Policy
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .WithExposedHeaders("X-Pagination");
                });
            });
        }

        /// <summary>
        /// Configure IIS configuration
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options => { });
        }

        /// <summary>
        /// Configure logger manager service
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        /// <summary>
        /// Configure Register Repository Manager
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        /// <summary>
        /// Configure Register Service Manager
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServiceManager(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
        }

        /// <summary>
        /// Registering DbContext
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("SqlConnection");
            //services.AddDbContextPool<DatabaseContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Scoped);
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Scoped);
            services.AddDbContext<AuthenticationContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Scoped);
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Scoped);
            services.AddDbContext<TenantContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Scoped);
        }

        /// <summary>
        /// Configure Identity
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
                o.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AuthenticationContext>()
            .AddDefaultTokenProviders();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureTenantResolver(this IServiceCollection services)
        {
            services.AddTransient<ITenantResolver, TenantResolver>();
            services.AddTransient<IUserAccountResolver, UserAccountResolver>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder)
        {
            return builder.AddMvcOptions(config => config.OutputFormatters.Add(new CsvOutputFormatter()));
        }

        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var systemTextJsonOutputFormatter = config.OutputFormatters
                        .OfType<SystemTextJsonOutputFormatter>()?
                        .FirstOrDefault();

                if (systemTextJsonOutputFormatter != null)
                {
                    systemTextJsonOutputFormatter.SupportedMediaTypes
                            .Add("application/vnd.sampledemo.hateoas+json");
                }

                var xmlOutputFormatter = config.OutputFormatters
                        .OfType<XmlDataContractSerializerOutputFormatter>()?
                        .FirstOrDefault();

                if (xmlOutputFormatter != null)
                {
                    xmlOutputFormatter.SupportedMediaTypes
                            .Add("application/vnd.sampledemo.hateoas+xml");
                }
            });
        }

        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            services.AddRateLimiter(opt =>
            {
                opt.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter("GlobalLimiter",
                    partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 5,
                        QueueLimit = 2,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        Window = TimeSpan.FromMinutes(1),
                    }));

                //opt.RejectionStatusCode = 429;

                opt.OnRejected = async (rejectContext, cancellationToken) =>
                {
                    rejectContext.HttpContext.Response.StatusCode = 429;

                    if (rejectContext.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        await rejectContext.HttpContext.Response.WriteAsync(
                            $"Too many request. Please try after {retryAfter.TotalSeconds} seconds.", cancellationToken);
                    }
                    else
                    {
                        await rejectContext.HttpContext.Response.WriteAsync(
                            $"Too many request. Please try again later.", cancellationToken);
                    }
                };
            });
        }

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
            }).AddMvc();
        }
    }
}
