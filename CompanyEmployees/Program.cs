using CompanyEmployees.Extensions;
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

builder.Services.AddControllers();

//Creates the app variable of the type WebApplication
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
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
