using Amazon.S3;
using Cartify.Application.Mappings;
using Cartify.Application.Services.Implementation;
using Cartify.Application.Services.Implementation.Authentication;
using Cartify.Application.Services.Implementation.Customer;
using Cartify.Application.Services.Implementation.Helper;
using Cartify.Application.Services.Implementation.Merchant;
using Cartify.Application.Services.Implementation.Profile;
using Cartify.Application.Services.Interfaces;
using Cartify.Application.Services.Interfaces.Authentication;
using Cartify.Application.Services.Interfaces.Customer;
using Cartify.Application.Services.Interfaces.Merchant;
using Cartify.Application.Services.Interfaces.Product;
using Cartify.Domain.Interfaces.Repositories;
using Cartify.Domain.Models;
using Cartify.Infrastructure.Implementation.Repository;
using Cartify.Infrastructure.Implementation.Services;
using Cartify.Infrastructure.Implementation.Services.Helper;
using Cartify.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Cartify.API
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 🧠 Load configuration sources
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Program>() // يقرأ الـAWS وJWT من User Secrets
                .AddEnvironmentVariables();

            // 🧾 Controllers
            builder.Services.AddControllers();

            // 🌐 CORS Policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://127.0.0.1:5500")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowGitHub",
                    policy => policy.WithOrigins("https://a7med3yad.github.io")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });
            // ayad is here


            // 🧱 Database Context
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 👤 Identity Configuration
            builder.Services.AddIdentityCore<TblUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // 👥 User & Auth Services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<IRegisterService, RegisterService>();
            builder.Services.AddScoped<ICreateJWTToken, CreateJWTToken>();
            builder.Services.AddScoped<IResetPassword, ResetPassword>();
            builder.Services.AddHttpContextAccessor();

            // ☁️ AWS S3 Configuration (manual secure setup)
            builder.Services.AddSingleton<IAmazonS3>(sp =>
            {
                var awsAccessKey = builder.Configuration["AWS:AccessKey"];
                var awsSecretKey = builder.Configuration["AWS:SecretKey"];
                var awsRegion = builder.Configuration["AWS:Region"] ?? "eu-central-1";

                var config = new AmazonS3Config
                {
                    RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(awsRegion)
                };

                return new AmazonS3Client(awsAccessKey, awsSecretKey, config);
            });
            builder.Services.AddScoped<IFileStorageService, S3FileStorageService>();

            // 🧱 Infrastructure Repositories
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<ICheckoutRepository, CheckoutRepository>();
            builder.Services.AddScoped<IOrdertrackingRepository, OrdertrackingRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // 📧 Helpers & Utilities
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<ICreateMerchantProfile, CreateMerchantProfile>();
            builder.Services.AddScoped<GetUserServices>();
			builder.Services.AddScoped<ISubmitTicket,SubmitTicket>();


            // 👤 Profile Services
            builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
            builder.Services.AddScoped<IProfileServices, ProfileServices>();
            builder.Services.AddScoped<IProfileService, ProfileService>();

            // 🛒 Order & Checkout Services
            builder.Services.AddScoped<ICheckoutService, CheckoutService>();
            builder.Services.AddScoped<IOrdertrackingservice, OrdertrackingService>();
            builder.Services.AddScoped<ICustomerOrderService, CustomerOrderService>();

            // 🛍️ Merchant Services
            builder.Services.AddScoped<IMerchantProductServices, MerchantProductServices>();
            builder.Services.AddScoped<IMerchantCategoryServices, MerchantCategoryServices>();
            builder.Services.AddScoped<IMerchantCustomerServices, MerchantCustomerServices>();
            builder.Services.AddScoped<IMerchantInventoryServices, MerchantInventoryServices>();
            builder.Services.AddScoped<IMerchantOrderServices, MerchantOrderServices>();
            builder.Services.AddScoped<IMerchantTransactionServices, MerchantTransactionServices>();
            builder.Services.AddScoped<IMerchantProfileServices, MerchantProfileServices>();

            // 🧭 Mapping + Configurations
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("Jwt"));
            builder.Services.Configure<SMTPSettings>(builder.Configuration.GetSection("Smtp"));

            // 🔑 JWT Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // 🧾 Swagger + OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                var filepath = Path.Combine(AppContext.BaseDirectory, "Cartify.API.xml");
                option.IncludeXmlComments(filepath);
                option.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Cartify API",
                        Version = "v1",
                        Description = "ASP.NET Core WebAPI for E-commerce Platform",
                        Contact = new OpenApiContact
                        {
                            Name = "Ahmed Ayad",
                            Email = "ahmed.ibrahim01974@gmail.com",
                        },
                    });
                option.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                            In = ParameterLocation.Header,
                            Name = "Authorization"
                        },
                        new List<string>()
                    }
                });
            });


            // 🚀 Build app
            var app = builder.Build();

            // 🧩 Middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseCors("AllowGitHub");
            }

            // ✅ Enable Swagger in all environments (Development & Production)
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cartify API v1");
                c.RoutePrefix = "swagger"; // Swagger will be available at /swagger
                c.DocumentTitle = "Cartify API Documentation";
                c.DefaultModelsExpandDepth(-1); // Hide models section by default
            });

            app.UseHttpsRedirection();
            app.UseCors("AllowGitHub");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            // 🟢 Health check
            app.MapGet("/", () => "✅ Cartify API is running successfully!");

            app.Run();
        }
    }
}
