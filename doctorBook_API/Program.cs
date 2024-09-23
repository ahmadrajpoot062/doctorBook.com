using Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configuring JWT
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:7096",
            ValidAudience = "https://localhost:7096",
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<Core.Interfaces.IAppointmentRepository, Infrastructure.AppointmentRepository>();
builder.Services.AddScoped<Core.Interfaces.IDoctorRepository, Infrastructure.DoctorRepository>();
builder.Services.AddScoped(typeof(Core.Interfaces.IRepository<>), typeof(Infrastructure.GenericRepository<>));

builder.Services.AddScoped(typeof(GenericServices<>));
builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<DoctorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
