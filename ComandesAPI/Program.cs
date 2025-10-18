using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ComandesAPI.Data;
using ComandesAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Registrar serveis d'autenticació
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Configurar autenticació JWT
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];
if (string.IsNullOrEmpty(jwtSecretKey))
{
    throw new InvalidOperationException("La clau secreta JWT no està configurada. Afegiu 'Jwt:SecretKey' a appsettings.json");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

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

    // Configurar autenticació JWT a Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Autenticació JWT. Introduïu 'Bearer' seguit del token. Exemple: 'Bearer {token}'",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
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
else
{
    // Només redirigir a HTTPS en producció
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Redirect root to Swagger UI when in Development or Swagger is enabled
app.MapGet("/", () => Results.Redirect("/swagger", permanent: false));

// Aplicar migracions d'EF Core automàticament a l'inici.
// Database.Migrate() crearà la base de dades si no existeix i aplicarà
// totes les migracions pendents automàticament.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetService<ILoggerFactory>()?.CreateLogger("Program");

    try
    {
        var db = services.GetRequiredService<ComandesAPI.Data.ComandesDbContext>();

        logger?.LogInformation("Comprovant migracions de base de dades...");

        // Migrate() crearà la BD si no existeix i aplicarà migracions pendents
        db.Database.Migrate();

        logger?.LogInformation("Base de dades actualitzada correctament.");
    }
    catch (Exception ex)
    {
        logger?.LogError(ex, "S'ha produït un error en migrar o inicialitzar la base de dades.");
        throw; // Relançar per evitar que l'aplicació s'iniciï amb errors de BD
    }
}

app.Run();

