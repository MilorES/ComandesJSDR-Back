using Microsoft.EntityFrameworkCore;
using ComandesAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Build connection string from Docker environment variables when provided,
// otherwise fall back to appsettings.json connection string.
string? dockerConn = null;
{
    // Common Docker environment variable names. Docker Compose can set these.
    var host = Environment.GetEnvironmentVariable("DB_HOST");
    var name = Environment.GetEnvironmentVariable("DB_NAME");
    var user = Environment.GetEnvironmentVariable("DB_USER");
    var pass = Environment.GetEnvironmentVariable("DB_PASS");

    if (!string.IsNullOrWhiteSpace(host) && !string.IsNullOrWhiteSpace(name) &&
        !string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(pass))
    {
        // Build a MySQL connection string
        dockerConn = $"Server={host};Database={name};User={user};Password={pass};";
    }

    // Also allow passing the full connection string via ConnectionStrings__DefaultConnection
    var overrideFull = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
    if (!string.IsNullOrWhiteSpace(overrideFull)) dockerConn = overrideFull;
}

var connectionString = dockerConn ?? builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine("Using connection string: " + connectionString);

// Add Entity Framework
builder.Services.AddDbContext<ComandesDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 21))
    ));

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API de Comandes JDSR",
        Version = "v1",
        Description = "API per gestionar articles i comandes",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "JDSR",
            Email = "info@jdsr.com"
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Comandes JDSR v1");
        c.RoutePrefix = "swagger"; // Serve Swagger UI at /swagger
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Redirect root to Swagger UI when in Development or Swagger is enabled
app.MapGet("/", () => Results.Redirect("/swagger", permanent: false));

// Apply EF Core migrations automatically on startup (development-friendly).
// This will create/update the database schema according to the Migrations folder.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<ComandesAPI.Data.ComandesDbContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetService<ILoggerFactory>()?.CreateLogger("Program");
        logger?.LogError(ex, "An error occurred while migrating or initializing the database.");
        // In production you might want to rethrow or handle differently
    }
}

app.Run();

