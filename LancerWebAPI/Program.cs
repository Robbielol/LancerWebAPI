// Program.cs in .NET 6+
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});

// Pull the connection string from appsettings.json (or AWS environment variables)
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoConnectionString");

// Register the MongoClient as a Singleton so the whole app can use it
builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));
builder.Services.AddSingleton<PlaceRepository>();
builder.Services.AddSingleton<SearchCacheRepository>();
builder.Services.AddSingleton<UserRepository>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<GoogleMapsAPIService>();
builder.Services.AddScoped<WebsiteServices>();
builder.Services.AddScoped<UserService>();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "API is healthy!");

app.Run();