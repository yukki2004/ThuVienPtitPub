
using ThuVienPtit.Src.Presention.ExceptionMiddleware;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ThuVienPtit.Src.Application.Behaviors;
using ThuVienPtit.Src.Application.Users.Command;
using ThuVienPtit.Src.Application.Users.Validators;
using ThuVienPtit.Src.Infrastructure.Data;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http.Features;
using ThuVienPtit.Src.Application.Semesters.Validators;
using ThuVienPtit.Src.Application.Courses.Validator;
using ThuVienPtit.Src.Application.Interface;
using ThuVienPtit.Src.Infrastructure.Respository;
using StackExchange.Redis;
using ThuVienPtit.Src.Application.DashBoard.Interface;
using ThuVienPtit.Src.Infrastructure.DashBoard.Repository;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ThuVienPtit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // database sql server
            builder.Services.AddDbContext<AppDataContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Authentication + JWT Bearer
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
            // MediatR
            // command và queris cho module user
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var config = builder.Configuration.GetSection("Redis");
                var options = new ConfigurationOptions
                {
                    EndPoints = { $"{config["Host"]}:{config["Port"]}" },
                    Password = config["Password"],
                };
                return ConnectionMultiplexer.Connect(options);
            });
            builder.Services.AddScoped<ICacheService, RedisCacheService>();


            //validator
            //user
            builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<LoginUserValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserValidator>();
            //semester
            builder.Services.AddValidatorsFromAssemblyContaining<CreadteSemesterValidator>();
            // courrse
            builder.Services.AddValidatorsFromAssemblyContaining<CreateCourseValidator>();
            // ValidationBehavior
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            // các interface cho module user
            builder.Services.AddScoped<ThuVienPtit.Src.Application.Users.Interface.IUserRespository, ThuVienPtit.Src.Infrastructure.Users.Respository.UserRespository>();
            builder.Services.AddScoped<ThuVienPtit.Src.Application.Users.Interface.IJwtTokenService, ThuVienPtit.Src.Infrastructure.Users.Service.JwtTokenService>();
            builder.Services.AddScoped<ThuVienPtit.Src.Application.Users.Interface.IPasswordHasher, ThuVienPtit.Src.Infrastructure.Users.Service.PasswordHasher>();
            builder.Services.AddScoped<ThuVienPtit.Src.Application.Users.Interface.IRefreshToken, ThuVienPtit.Src.Infrastructure.Users.Respository.RefreshToken>();
            builder.Services.AddScoped<ThuVienPtit.Src.Application.Users.Interface.IFileStorage, ThuVienPtit.Src.Infrastructure.Users.Service.FileStorage>();
            builder.Services.AddScoped<ThuVienPtit.Src.Application.Users.Interface.IPasswordResetTokenRespository, ThuVienPtit.Src.Infrastructure.Users.Respository.PasswordResetToken>();
            builder.Services.AddScoped<ThuVienPtit.Src.Application.Users.Interface.IEmailSenderService, ThuVienPtit.Src.Infrastructure.Users.Service.EmailSenderService>();
            // các interface cho module semesters
            builder.Services.AddScoped<ThuVienPtit.Src.Application.Semesters.Interface.ISemesterRespository, ThuVienPtit.Src.Infrastructure.Semesters.Respository.SemesterRespository>();
            builder.Services.AddScoped<ThuVienPtit.Src.Application.Semesters.Interface.IFileStorageService, ThuVienPtit.Src.Infrastructure.Semesters.Service.FileStorageService>();
            // interface cho module courses
            builder.Services.AddScoped<ThuVienPtit.Src.Application.Courses.Interface.ICourseRespository, ThuVienPtit.Src.Infrastructure.Courses.Respository.CourseRespository>();
            builder.Services.AddScoped<ThuVienPtit.Src.Application.Courses.Interface.IFileStorageService, ThuVienPtit.Src.Infrastructure.Courses.Service.FileStorage>();
            // interface cho module documents
            builder.Services.AddScoped<ThuVienPtit.Src.Application.Documents.Interface.IDocumentRespository, ThuVienPtit.Src.Infrastructure.Documents.Respository.DocumentRespository>();
            builder.Services.AddScoped<ThuVienPtit.Src.Application.Documents.Interface.IFileStorageService, ThuVienPtit.Src.Infrastructure.Documents.Service.FileStorageService>();
            builder.Services.AddScoped<ThuVienPtit.Src.Application.Documents.Interface.ISlugDocumentService, ThuVienPtit.Src.Infrastructure.Documents.Service.SlugDocumentService>();
            // interface cho module tags
            builder.Services.AddScoped<ThuVienPtit.Src.Application.Tags.Interface.ITagRespository, ThuVienPtit.Src.Infrastructure.Tags.Respository.TagRespository>();
            // interface cho helper
            builder.Services.AddScoped<ISlugHelper, SlugHelper>();
            // interface cho dashboard 
            builder.Services.AddScoped<IDashBoardRespository, DashBoardRepository>();
            // CORS giúp kết nối fontend
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://172.11.202.246:5173", "http://localhost:5173","http://172.11.200.237:5173", "http://192.168.0.101:5173", "http://172.11.124.161:5173", "http://172.22.112.1:5173", "http://172.11.125.106:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });



            // builder các service

            builder.Services.AddControllers();
            builder.Services.AddAuthorization();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ThuVienPtit API",
                    Version = "v1",
                    Description = "Tài liệu API cho hệ thống Quản lý Thư viện PTIT"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Nhập token theo dạng: Bearer {your JWT token}"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 5L * 1024 * 1024 * 1024;
            });

            // 2. Tăng giới hạn cho Kestrel
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 5L * 1024 * 1024 * 1024;
            });

            //builder.WebHost.ConfigureKestrel(options =>
            //{
            //    options.ListenAnyIP(7188); 
            //});
            builder.Services.AddEndpointsApiExplorer();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();

            var app = builder.Build();
            var fileServerPath = builder.Configuration["FileStorage:RootPath"];
            if (!string.IsNullOrWhiteSpace(fileServerPath) && Directory.Exists(fileServerPath))
            {
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(fileServerPath),
                    RequestPath = "/files",
                     OnPrepareResponse = ctx =>
                     {
                         // Thêm CORS headers cho static files (PDF,...)
                         //ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "http://localhost:5173");
                         //ctx.Context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, OPTIONS");
                         //ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type");
                     }
                });
            }

            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ThuVienPtit API v1");
                    c.RoutePrefix = string.Empty;
                });
            }
            //pipeline
            app.UseMiddleware<ExceptionMiddleware>();
            //app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
