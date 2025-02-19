using Microsoft.EntityFrameworkCore;
using PoetryLovers.Data;
using PoetryLovers.Entities;
using PoetryLovers.IServices;
using PoetryLovers.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient("poetryDb", (httpClient) => 
{
    httpClient.BaseAddress = new Uri("https://poetrydb.org/");
});

builder.Services.AddScoped<PoetryDbService>();
builder.Services.AddScoped<IPoemRepo, PoemRepo>();

var env = builder.Environment;

if (env.IsDevelopment())
{
    builder.Services.AddDbContext<PoemContext>(opt => {
        var connectionString = builder.Configuration.GetConnectionString("Sqlite");
        opt.UseSqlite(connectionString);
    });
}

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<PoemContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
