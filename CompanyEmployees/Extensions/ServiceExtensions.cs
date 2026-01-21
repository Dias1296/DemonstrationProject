using Contracts;
using Logger_Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
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
                    .AllowAnyHeader()
                    .WithExposedHeaders("X-Pagination"));
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

        //Adds custom formatter to the list of OutputFormatters
        public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder) =>
            builder.AddMvcOptions(config => config.OutputFormatters.Add(new CsvOutputFormatter()));

        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var systemTextJsonOutputFormatter = config.OutputFormatters
                    .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();

                if (systemTextJsonOutputFormatter != null)
                {
                    systemTextJsonOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.fdad.hateoas+json");
                    systemTextJsonOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.fdad.apiroot+json");
                }

                var xmlOutputFormatter = config.OutputFormatters
                                         .OfType<XmlDataContractSerializerOutputFormatter>()?
                                         .FirstOrDefault();

                if (xmlOutputFormatter != null)
                {
                    xmlOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.fdad.hateoas+xml");
                    xmlOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.fdad.apiroot+xml");
                }
            });
        }
    }
}
