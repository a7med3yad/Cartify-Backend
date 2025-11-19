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

            // -----------------------------
            // Load configuration
            // -----------------------------
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables();


            // -----------------------------
            // Controllers
            // -----------------------------
            builder.Services.AddControllers();

<<<<<<< HEAD

            // -----------------------------
            // CORS (DEBUG MODE - AllowAnyOrigin)
            // IMPORTANT: After deployment, check response headers.
            // -----------------------------
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CartifyCors", policy =>
                {
                    policy
                        .AllowAnyOrigin()      // أو WithOrigins(...)
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });


            // -----------------------------
            // Database Context
            // -----------------------------
            builder.Services.AddDbContext<AppDbContext>(options =>
=======
			// 🌐 CORS Policy
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowOrigins", policy =>
				{
					policy.WithOrigins(
							"http://127.0.0.1:5500",
							"https://a7med3yad.github.io")
						  .AllowAnyMethod()
						  .AllowAnyHeader()
						  .AllowCredentials();
				});
			});

			// ayad is here


			// 🧱 Database Context
			builder.Services.AddDbContext<AppDbContext>(options =>
>>>>>>> main
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            // -----------------------------
            // Identity
            // -----------------------------
            builder.Services.AddIdentityCore<TblUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


            // -----------------------------
            // User Services
            // -----------------------------
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<IRegisterService, RegisterService>();
            builder.Services.AddScoped<ICreateJWTToken, CreateJWTToken>();
            builder.Services.AddScoped<IResetPassword, ResetPassword>();
            builder.Services.AddHttpContextAccessor();


            // -----------------------------
            // AWS S3
            // -----------------------------
            builder.Services.AddSingleton<IAmazonS3>(sp =>
            {
                var access = builder.Configuration["AWS:AccessKey"];
                var secret = builder.Configuration["AWS:SecretKey"];
                var region = builder.Configuration["AWS:Region"] ?? "eu-central-1";

                return new AmazonS3Client(access, secret, new AmazonS3Config
                {
                    RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region)
                });
            });

            builder.Services.AddScoped<IFileStorageService, S3FileStorageService>();


            // -----------------------------
            // Repositories
            // -----------------------------
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<ICheckoutRepository, CheckoutRepository>();
            builder.Services.AddScoped<IOrdertrackingRepository, OrdertrackingRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            // -----------------------------
            // App Services
            // -----------------------------
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<ICreateMerchantProfile, CreateMerchantProfile>();
            builder.Services.AddScoped<GetUserServices>();
<<<<<<< HEAD
=======
			builder.Services.AddScoped<ISubmitTicket,SubmitTicket>();

            // 👤 Profile Services
>>>>>>> main
            builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
            builder.Services.AddScoped<IProfileServices, ProfileServices>();
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddScoped<ICheckoutService, CheckoutService>();
            builder.Services.AddScoped<IOrdertrackingservice, OrdertrackingService>();
            builder.Services.AddScoped<ICustomerOrderService, CustomerOrderService>();

            builder.Services.AddScoped<IMerchantProductServices, MerchantProductServices>();
            builder.Services.AddScoped<IMerchantCategoryServices, MerchantCategoryServices>();
            builder.Services.AddScoped <IMerchantCustomerServices, MerchantCustomerServices> ();
            builder.Services.AddScoped<IMerchantInventoryServices, MerchantInventoryServices>();
            builder.Services.AddScoped<IMerchantOrderServices, MerchantOrderServices>();
            builder.Services.AddScoped<IMerchantTransactionServices, MerchantTransactionServices>();
            builder.Services.AddScoped<IMerchantProfileServices, MerchantProfileServices>();


            // -----------------------------
            // Mapping + Configs
            // -----------------------------
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("Jwt"));
            builder.Services.Configure<SMTPSettings>(builder.Configuration.GetSection("Smtp"));


            // -----------------------------
            // JWT Authentication
            // -----------------------------
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


            // -----------------------------
            // Swagger
            // -----------------------------
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                var xml = Path.Combine(AppContext.BaseDirectory, "Cartify.API.xml");
                option.IncludeXmlComments(xml);

                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Cartify API",
                    Version = "v1",
                    Description = "ASP.NET Core WebAPI for Cartify Platform"
                });

                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });


            // -----------------------------
            // Build App
            // -----------------------------
            var app = builder.Build();

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cartify API v1");
                c.RoutePrefix = "swagger";
            });


            app.UseHttpsRedirection();
<<<<<<< HEAD
            app.UseCors("CartifyCors"); // ✅
            app.UseAuthentication();
=======
			app.UseCors("AllowOrigins");
			app.UseAuthentication();
>>>>>>> main
            app.UseAuthorization();
            app.MapControllers();

            app.MapGet("/", () => "✅ Cartify API is running with CORS enabled (DEBUG MODE)!");

            app.Run();
        }
    }
}
