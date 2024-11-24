using API.Iserviecs;
using API.Services;
using DBcontext.Models;
using DBcontext.Viewmodel;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Đăng ký SmtpSettings từ appsettings.json vào DI container
var smtpSettings = builder.Configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
builder.Services.AddSingleton(smtpSettings);

// Đăng ký dịch vụ MailServices (không cần đăng ký SmtpClient ở đây)
builder.Services.AddTransient<IMailServices, MailServices>();



// Đăng ký dịch vụ IHopDong với lớp thực thi HopDongService
builder.Services.AddScoped<IHopDongServices, HopdongServices>();


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
