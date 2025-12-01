using Contracts;
using Logger_Service;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service;
using Service.Contracts;

namespace CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {
        //Configures service to allow any origin/method/header
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
                //Examples for specific origins/methods/headers
                //WithOrigins("example URL") -> Only allows requests from the example URL origin.
                //WithMethods("POST", "GET") -> Only allows the specific HTTP methods.
                //WithHeaders("accept", "content-type") -> Only allows specific headers.
            });

        //Configures IIS integration for later deployment in IIS
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {
                //Default configuration for IIS
            });

        //Configures the logger service
        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerManager, LoggerManager>();

        //Configures manager class
        public static void ConfigureRepositoryManager(this IServiceCollection services) => 
            services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureServiceManager(this IServiceCollection services) => 
            services.AddScoped<IServiceManager, ServiceManager>();

        public static void ConfigureSqlContext(this IServiceCollection services, 
            IConfiguration configuration) => 
            services.AddDbContext<RepositoryContext>(opts => 
                opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
    }
}
