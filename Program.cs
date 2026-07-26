using ApiTienda.Constants;
using ApiTienda.Database;
using ApiTienda.Extensions;
using ApiTienda.Filters;
using ApiTienda.Helpers;
using ApiTienda.Services;
using ApiTienda.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<TiendaDbContext>(options => 
    options.UseSqlServer(builder.Configuration
    .GetConnectionString("DefaultConnection")));

// Acceder al contexto de la peticin HTTP
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddControllers( options => 
{
    options.Filters.Add(typeof(ValidarEstadoDeModeloAtributo));
});

builder.Services.Configure<ApiBehaviorOptions>(options => 
{
    options.SuppressModelStateInvalidFilter = true;
});

// Registrar servicios personalizados aquí
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddTransient<IRolesService, RolesService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<ITransactionService, TransactionService>();
builder.Services.AddTransient<IDataInitializationService, DataInitializationService>();
builder.Services.AddTransient<IFileService, FileService>();

builder.Services.AddCorsConfiguration(builder.Configuration);
builder.Services.AddAuthenticationConfig(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API de la Tienda",
        Description = "API para la gestión de productos, categorías, órdenes, usuarios y roles."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

                // Configuración para JWT Bearer Authentication en Swagger UI
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Ingrese su token JWT en el campo de texto de abajo.\n\nEl prefijo 'Bearer' se agregará automáticamente.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Inicializar base de datos y datos por defecto
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<TiendaDbContext>();

        // Aplicar migraciones pendientes (crea la base de datos si no existe)
        await context.Database.MigrateAsync();
        Console.WriteLine("🗄️ Migraciones aplicadas correctamente");

        // Inicializar datos por defecto solo si la base de datos está vacía (primera vez)
        if (!await context.Users.AnyAsync())
        {
            var dataInitializationService = scope.ServiceProvider.GetRequiredService<IDataInitializationService>();
            var result = await dataInitializationService.InitializeDataAsync();

            if (result.Success)
            {
                Console.WriteLine("✅ Datos inicializados automáticamente");
                Console.WriteLine($"📊 Resumen: {result.Message}");
                if (result.Details != null)
                {
                    Console.WriteLine($"   - Roles creados: {result.Details.RolesCreated}");
                    Console.WriteLine($"   - Usuarios creados: {result.Details.UsersCreated}");
                    Console.WriteLine($"   - Categorías de productos creadas: {result.Details.ProductCategoriesCreated}");
                }
                Console.WriteLine("👥 Usuarios disponibles:");
                Console.WriteLine("   - admin@admin.com / admin (ADMINISTRADOR)");
                Console.WriteLine("   - vendedor@vendedor.com / vendedor (VENDEDOR)");
                Console.WriteLine("   - cliente@cliente.com / cliente (CLIENTE)");
            }
            else
            {
                Console.WriteLine($"❌ Error al inicializar datos: {result.Message}");
            }
        }
        else
        {
            Console.WriteLine("ℹ️ La base de datos ya tiene datos, se omite el seed");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error durante la inicialización de la base de datos: {ex.Message}");
        // No lanzar la excepción para que la aplicación pueda continuar
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API de la Tienda v1");
        options.RoutePrefix = string.Empty; // Para que Swagger UI se cargue en la raíz de la URL
        options.ConfigObject.AdditionalItems["syntaxHighlight"] = false;
        options.ConfigObject.AdditionalItems["displayRequestDuration"] = true;
        // Configuración para expandir automáticamente las operaciones y modelos
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        options.DefaultModelsExpandDepth(-1); // Para no mostrar los modelos por defecto
    });
    
    // Redirecciones personalizadas para Swagger
    app.MapGet("/swagger/index.html", () => Results.Redirect("/index.html"));
    app.MapGet("/swagger", () => Results.Redirect("/index.html"));
}

app.UseHttpsRedirection();

// Configurar archivos estáticos para servir imágenes
app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

