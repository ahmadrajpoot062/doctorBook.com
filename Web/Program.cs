using Application.Services;
using Core.Entities;
using Core.Interfaces;
using doctorBook.com.Data;
using doctorBook.com.Models;
using Infrastructure;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using static Dapper.SqlMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<myAppUsers>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.AllowedForNewUsers = true;
}
)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireClaim(ClaimTypes.Email, "Admin@doctorBook.com"));
    options.AddPolicy("PatientPolicy", policy => policy.RequireClaim("Role", "Patient"));
    options.AddPolicy("DoctorPolicy", policy => policy.RequireClaim("Role", "Doctor"));
});


builder.Services.AddScoped<Core.Interfaces.IAppointmentRepository, Infrastructure.AppointmentRepository>();
builder.Services.AddScoped<Core.Interfaces.IDoctorRepository, Infrastructure.DoctorRepository>();
builder.Services.AddScoped(typeof(Core.Interfaces.IRepository<>), typeof(Infrastructure.GenericRepository<>));

builder.Services.AddScoped(typeof(GenericServices<>));
builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<DoctorService>();

builder.Services.AddDistributedMemoryCache();

// Add in-memory caching service
builder.Services.AddMemoryCache();

// Add response caching service
builder.Services.AddResponseCaching();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR(); // Add SignalR services

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<Infrastructure.Hubs.AppointmentHub>("/appointmentHub"); // Map SignalR hub

app.MapRazorPages();

app.Run();
