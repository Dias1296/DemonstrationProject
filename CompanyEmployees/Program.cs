using ActionFilters;
using CompanyEmployees.Extensions;
using CompanyEmployees.Utility;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using NLog;
using Service.Contracts;
using Service.DataShaping;
using Shared.DataTransferObjects;

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
builder.Services.ConfigureVersioning();

//Suppresses the default model state validation that is implemented due to the existence
//of the [ApiController] attribute in all API controllers.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

//Configures support for JSON Patch using Newtonsoft.Json
//while leaving the other formatters unchanged
NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
    new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
    .Services.BuildServiceProvider()
    .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
    .OfType<NewtonsoftJsonPatchInputFormatter>().First();

builder.Services.AddScoped<ValidationFilterAttribute>();

builder.Services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();

builder.Services.AddScoped<IEmployeeLinks, EmployeeLinks>();

//Loads controllers from another assembly (another project)
builder.Services.AddControllers(config => {
    config.RespectBrowserAcceptHeader = true;   //Sets the server flag for content negotiation (Accept Header)
    config.ReturnHttpNotAcceptable = true;      //Sets the server flag to return a Not Acceptable response for non-supported response formats.
    config.InputFormatters.Insert(0, GetJsonPatchInputFormatter()); //Places JsonPatchInputFormatter at the index 0 in the InputFormatters list.
}).AddXmlDataContractSerializerFormatters()
  .AddCustomCSVFormatter()
  .AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);

//Registers media types with HATEOAS
builder.Services.AddCustomMediaTypes();

//Registers media type filter
builder.Services.AddScoped<ValidateMediaTypeAttribute>();

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

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
