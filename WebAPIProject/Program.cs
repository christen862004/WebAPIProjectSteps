
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebAPIProject.Models;

namespace WebAPIProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options=>options.SuppressModelStateInvalidFilter=true);

            builder.Services.AddCors(corsoptions =>
            {
                corsoptions.AddPolicy("MyPolicy", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                });
            });

            builder.Services.AddDbContext<CompanyContext>(options =>
                options.UseSqlServer("Data Source=.;Initial Catalog=WebAPISteps;Integrated Security=True;Encrypt=False")
            );
          
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<CompanyContext>();//cookie

            //setting check ==>token
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme= JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;//
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer=true,
                    ValidIssuer= builder.Configuration["JWT:Iss"],
                    ValidateAudience=true,
                    ValidAudience= builder.Configuration["JWT:Aud"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
           
            builder.Services.AddEndpointsApiExplorer();
            #region Swagger Setting
            
            builder.Services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation    
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ASP.NET 8 Web API",
                    Description = " Steps Project"
                });
                // To Enable authorization using Swagger (JWT)    
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] {}
                    }
                    });
            });

            #endregion
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles(); //url ==>wwwroot handel

            //app.UseRouting();//middleware deafult one
            app.UseCors("MyPolicy");
            //app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
