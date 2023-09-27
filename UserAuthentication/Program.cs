using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserAuthentication.Data;
using UserAuthentication.Entities;
using UserAuthentication.Interface;
using UserAuthentication.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserAuthenticationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UserData")));
builder.Services.AddScoped<IRepository<RegistrationTable>, EFRepository<RegistrationTable>>();
builder.Services.AddScoped<IRepository<LoginTable>, EFRepository<LoginTable>>();
builder.Services.AddScoped<IUserService, UserService>();

//Add Authentication
builder.Services.AddAuthentication(option =>
{
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    //Add Jwt Bearer
  .AddJwtBearer(options =>
   {
       options.SaveToken = true;
       options.RequireHttpsMetadata = false;
       options.TokenValidationParameters = new TokenValidationParameters()
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidIssuer = builder.Configuration.GetValue<string>("JWT:ValidIssuer"),
           ValidAudience = builder.Configuration.GetValue<string>("JWT:ValidAudience"),
           ClockSkew = TimeSpan.FromMinutes(0)
       };
   });


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.LoginPath = "/authentication/GoogleLogin";
    })
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration.GetValue<string>("Authentication:Google:ClientId");
        options.ClientSecret = builder.Configuration.GetValue<string>("Authentication:Google:ClientSecret");
    });


var app = builder.Build(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
