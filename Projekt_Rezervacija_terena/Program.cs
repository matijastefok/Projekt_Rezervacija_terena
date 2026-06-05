using Microsoft.EntityFrameworkCore;
using Projekt_Rezervacija_terena.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. DODANO: Povezivanje baze podataka (ApplicationDbContext) s Connection Stringom iz appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Dodavanje servisa za kontrolere i poglede (tvoj postojeći kod)
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();