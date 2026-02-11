using Microsoft.EntityFrameworkCore;
using CRUDTRABAJADORES.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//CADENA DE CONEXION CON MI BASE DE DATOS
builder.Services.AddDbContext<TrabajadoresPruebaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL"))
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
