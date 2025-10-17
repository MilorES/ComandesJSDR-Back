using Microsoft.EntityFrameworkCore;
using ComandesAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configure CORS to allow requests from the frontend
// This block allows the frontend at http://localhost:5173
// to communicate with the API without being blocked by CORS

var MyAllowFrontend = "_myAllowFrontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowFrontend, policy =>
    {
        policy.WithOrigins("http://localhost:5173") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllers();

// Build connection string from Docker environment variables when provided,
// otherwise fall back to appsettings.json connection string.
string? dockerConn = null;
{
    var host = Environment.GetEnvironmentVariable("DB_HOST");
    var name = Environment.GetEnvironmentVariable("DB_NAME");
    var user = Environment.GetEnvironmentVariable("DB_USER");
    var pass = Environment.GetEnvironmentVariable("DB_PASS");

    if (!string.IsNullOrWhiteSpace(host) && !string.IsNullOrWhiteSpace(name) &&
        !string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(pass))
    {
        dockerConn = $"Server={host};Database={name};User={user};Password={pass};";
    }

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

// 🔹 Habilitar CORS (antes de UseAuthorization)
app.UseCors(MyAllowFrontend);

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Redirect root to Swagger UI when in Development or Swagger is enabled
app.MapGet("/", () => Results.Redirect("/swagger", permanent: false));

// Apply EF Core migrations automatically on startup (development-friendly)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<ComandesDbContext>();
        db.Database.Migrate();

    }
    catch (Exception ex)
    {
        var logger = services.GetService<ILoggerFactory>()?.CreateLogger("Program");
        logger?.LogError(ex, "An error occurred while migrating or initializing the database.");
    }
}

app.Run();
