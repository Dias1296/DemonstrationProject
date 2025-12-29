using CompanyEmployees.Extensions;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Deprecated
//LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), 
//    "/nlog.config"));
//Logger configuration
LogManager.Setup().LoadConfigurationFromFile("nlog.config");

builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));

//Loads controllers from another assembly (another project)
builder.Services.AddControllers(config => {
    config.RespectBrowserAcceptHeader = true;   //Sets the server flag for content negotiation (Accept Header)
    config.ReturnHttpNotAcceptable = true;      //Sets the server flag to return a Not Acceptable response for non-supported response formats.
}).AddXmlDataContractSerializerFormatters()
  .AddCustomCSVFormatter()
  .AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);

//Creates the app variable of the type WebApplication
var app = builder.Build();

//Extract the ILoggerManager service inside the logger variable.
//Extraction is done after the build because the Build method builds the WebApplication
//and registers all the services added with IOC
var logger = app.Services.GetRequiredService<ILoggerManager>();
//Pass the logger service to the ConfigureExceptionHandler method
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
    //Adds middleware for using HSTS
    app.UseHsts();

app.UseHttpsRedirection();

//Enables using static files for the request
app.UseStaticFiles();

//Forwards proxy headers to the current request
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
