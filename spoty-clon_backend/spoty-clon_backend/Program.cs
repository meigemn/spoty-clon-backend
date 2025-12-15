using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using spoty_clon_backend.Models.Context; 
using spoty_clon_backend.Services; 
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// 1. CONFIGURACIÓN DE BASE DE DATOS Y ENTITY FRAMEWORK CORE (EF CORE)


// Obtener la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Configurar el DbContext para PostgreSQL
builder.Services.AddDbContext<MeigemnDbContext>(options =>
    options.UseNpgsql(connectionString));


// 2. CONFIGURACIÓN DE IDENTITY

// Usamos IdentityUser directamente ya que no se personalizará
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Políticas de contraseña
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    // Opciones de inicio de sesión
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true; 
})
.AddEntityFrameworkStores<MeigemnDbContext>() // Enlaza Identity con EF Core
.AddDefaultTokenProviders();


// 3. CONFIGURACIÓN JWT (Ya la tenías, pero la movemos arriba del todo)

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key not configured"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // <-- TODO: Cambiar a TRUE en producción
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// =================================================================
// 4. INYECCIÓN DE DEPENDENCIAS (Servicios, UoW, etc.)
// =================================================================

// Registrar el servicio de usuarios que me pasaste (ajusta si tienes otros nombres)
// NOTA: Si IUnitOfWork no existe, o si tiene un ciclo de vida diferente, ajusta esto
builder.Services.AddScoped<UserService, UserService>();
// builder.Services.AddScoped<IUnitOfWork, MeigemnUnitOfWork>(); // Ejemplo si tienes un UoW

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 5. PIPELINE DE SOLICITUDES (Middlewares)

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Authentication debe ir antes de Authorization 
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// 6. SEEDING (Inicialización de datos) - Opcional, pero recomendado

// Este bloque asegura que los roles "Admin" y "User" existan en la BD
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roleNames = { "Admin", "User" };

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

app.Run();