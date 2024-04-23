using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PortalEmpleoBackend.Models;
using PortalEmpleoDB;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PortalEmpleoDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSwaggerGen();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // Para que la ruta raíz (/) redirija a Swagger
});

//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<PortalEmpleoDbContext>();
//    context.Database.Migrate();
//}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "swagger",
    pattern: "swagger",
    defaults: new { controller = "Swagger", action = "Index" }
);

app.Run();
