using Microsoft.EntityFrameworkCore;
using Waracle_Hotels.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

string? connection = builder.Configuration.GetConnectionString("CONNECTIONSTRING");
builder.Services.AddDbContext<HotelContext>(options => options.UseSqlServer(connection));

var app = builder.Build();

app.MapOpenApi().CacheOutput();
app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "v1"); });

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Here is the hotel API");

app.Run();
