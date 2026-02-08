using Domain.Data;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Domain.Repository;
using Domain.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(Options =>
Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IDressRepository, DressRepository>();
builder.Services.AddScoped<IDressService, DressService>();
builder.Services.AddScoped<IPriceCategoryRepository, PriceCategoryRepository>();
builder.Services.AddScoped<IPriceCategoryService, PriceCategoryService>();
builder.Services.AddScoped<IDressCategoryRepository, DressCategoryRepository>();
builder.Services.AddScoped<IDressCategoryService, DressCategoryService>();
//builder.Services.AddScoped<IOrderRepository, OrderRepository>();
//builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (!context.Users.Any(u => u.Role == UserRole.Admin))
    {
        var admin = new User
        {
            Name = "Stylebay",
            Email = "stylebay.noreply@gmail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = UserRole.Admin,

            Address = "Admin Office",
            City = "Kochi",
            State = "Kerala",
            PinCode = "682001",
            PhoneNumber = "9999999990"
        };

        context.Users.Add(admin);
        context.SaveChanges();
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
