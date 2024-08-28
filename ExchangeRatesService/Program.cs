using ExchangeRatesService.Models;
using ExchangeRatesService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Access the environment
var environment = builder.Environment;

// Load configurations based on the environment
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

var conn = builder.Configuration.GetConnectionString("db_connection");
builder.Services.AddDbContext<CurrenciesDBContext>(options => options.UseNpgsql(conn));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();

// configure graphql
builder.Services
    .AddGraphQLServer()
    // similar to the above. adds services to query/mutation resolver DI and avoids needing to add [service]
    .RegisterDbContext<CurrenciesDBContext>(DbContextKind.Synchronized)
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddType<CurrencyPair>();

builder.Services.AddHostedService<TimedHostedService>();

builder.Services.AddScoped<IExChangeService, ExChangeService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    Console.WriteLine("Running in Development mode");
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Custom error handling in Production
    Console.WriteLine("Running in Production mode");
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
    options.RoutePrefix = string.Empty; // Set the Swagger UI at the root URL
});
app.MapGraphQL();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.Run();

/// <summary>
/// The 'Program' class is a special class that always exists in a .net application
/// Here we are extending it and using it for thread-safe global variables. Things that are stateless and will not need mocking in tests.
/// </summary>
// public partial class Program
// {
//     /// <summary>
//     /// HttpClient is thread safe in .net. We only need a single instance per application.
//     /// </summary>
//     public static readonly HttpClient httpClient = new();

//     /// <summary>
//     /// An easy way to check if debug mode is enabled without causing ugly code and formatting issues
//     /// </summary>
//     public static bool isDebug =
// #if DEBUG
//         true;
// #else
//         false;
// #endif
// }