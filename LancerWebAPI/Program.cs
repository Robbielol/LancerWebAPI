// Program.cs in .NET 6+
var builder = WebApplication.CreateBuilder(args);

// Define a specific CORS policy
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000", // Your React app's origin
                                             "https://localhost:3000",// Your React app's origin if HTTPS
                                             "http://localhost:5173", // Another common React dev port
                                             "https://localhost:5173") // If React is on HTTPS
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
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

app.UseCors(myAllowSpecificOrigins); // Apply the CORS policy

app.UseAuthorization();

app.MapControllers();

app.Run();