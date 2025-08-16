using Microsoft.EntityFrameworkCore;
using Waracle_Hotels.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

string? connection = builder.Configuration.GetConnectionString("CONNECTIONSTRING");
/*if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    connection = builder.Configuration.GetConnectionString("CONNECTIONSTRING");
}
else
{
    connection = Environment.GetEnvironmentVariable("CONNECTIONSTRING");
}*/

builder.Services.AddDbContext<HotelContext>(options => options.UseSqlServer(connection));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Here is the hotel API");

app.Run();
