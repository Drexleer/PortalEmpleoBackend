using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PortalEmpleoBackend.Models;
using PortalEmpleoDB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PortalEmpleoDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Verificar la conexi�n a la base de datos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<PortalEmpleoDbContext>();

        if (dbContext.Database.CanConnect())
        {
            Console.WriteLine("Conexi�n establecida correctamente");
        }
        else
        {
            Console.WriteLine("No se pudo establecer la conexi�n");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error al verificar la conexi�n a la base de datos: " + ex.Message);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
