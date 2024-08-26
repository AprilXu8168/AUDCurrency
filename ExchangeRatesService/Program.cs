using ExchangeRatesService.Models;
using ExchangeRatesService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();

// has to be scoped as it is constructing a dbContext, which is itself scoped.
builder.Services.AddScoped<IEXChangeService, EXChangeService>();
builder.Services.AddHostedService<TimedHostedService>();

// configure graphql
builder.Services
    .AddGraphQLServer()
    // similar to the above. adds services to query/mutation resolver DI and avoids needing to add [service]
    .RegisterDbContext<CurrenciesDBContext>(DbContextKind.Synchronized)
    .AddQueryType<Query>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

var conn = builder.Configuration.GetConnectionString("db_connection");
builder.Services.AddDbContext<CurrenciesDBContext>(options => options.UseNpgsql(conn));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
        options.RoutePrefix = string.Empty; // Set the Swagger UI at the root URL
    });
    app.MapGraphQL();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.Run();

/// <summary>
/// The 'Program' class is a special class that always exists in a .net application
/// Here we are extending it and using it for thread-safe global variables. Things that are stateless and will not need mocking in tests.
/// </summary>
public partial class Program
{
    /// <summary>
    /// HttpClient is thread safe in .net. We only need a single instance per application.
    /// </summary>
    public static readonly HttpClient httpClient = new();

    /// <summary>
    /// An easy way to check if debug mode is enabled without causing ugly code and formatting issues
    /// </summary>
    public static bool isDebug =
#if DEBUG
        true;
#else
        false;
#endif
}