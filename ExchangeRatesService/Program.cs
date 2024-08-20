using Microsoft.EntityFrameworkCore;
using ExchangeRatesService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();


// builder.Services.AddDbContext<CurrenciesDBContext>(opt =>
    // opt.UseInMemoryDatabase("CurrencyList"));
var conn = builder.Configuration.GetConnectionString("db_connection");
    builder.Services.AddDbContext<CurrenciesDBContext>(options =>
    options.UseNpgsql(conn));

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
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
// app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.Run();