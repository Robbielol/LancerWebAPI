// Program.cs in .NET 6+
using LancerWebAPI.Database;
using LancerWebAPI.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Define a specific CORS policy
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
IConfiguration config = builder.Configuration;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Pull the connection string from appsettings.json (or AWS environment variables)
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoConnectionString");

// Register the MongoClient as a Singleton so the whole app can use it
builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));
builder.Services.AddSingleton<PlaceRepository>();
builder.Services.AddSingleton<SearchCacheRepository>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<GoogleMapsAPIService>();
builder.Services.AddScoped<WebsiteServices>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting(); // Ensure UseRouting is before UseCors and UseAuthorization

app.UseCors("AllowAll"); // Apply the CORS policy

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "API is healthy!");

app.Run();