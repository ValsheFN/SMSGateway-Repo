using Hangfire;
using Hangfire.Console;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SMSGateway.Server.DataSeeding;
using SMSGateway.Server.Infrastructure;
using SMSGateway.Server.Models;
using SMSGateway.Server.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SMSGateway.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDBContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly("SMSGateway.Server");
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HangfireApplication", Version = "v1" });
            });

            services.AddHangfire(options =>
            {
                options.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"));
                options.UseConsole();
            });

            services.AddHangfireServer();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["AuthSettings:Audience"],
                    ValidIssuer = Configuration["AuthSettings:Issuer"],
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthSettings:Key"])),
                    ValidateIssuerSigningKey = true
                }
            );

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddScoped(sp => new AuthOptions
            {
                Audience = Configuration["AuthSettings:Audience"],
                Issuer = Configuration["AuthSettings:Issuer"],
                Key = Configuration["AuthSettings:Key"]
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IContactGroupService, ContactGroupService>();
            services.AddScoped<ISmsTemplateService, SmsTemplateService>();
            services.AddScoped<ITopUpService, TopUpService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IHistoryService, HistoryService>();
            services.AddScoped<ISchedulerService, SchedulerService>();
            services.AddHttpContextAccessor();

            services.AddScoped(sp => 
            {
                var httpContext = sp.GetService<IHttpContextAccessor>().HttpContext;
                var identityOptions = new IdentityOption();

                if (httpContext.User.Identity.IsAuthenticated)
                {
                    string id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    string role = httpContext.User.FindFirst(ClaimTypes.Role).Value;

                    identityOptions.UserId = id;
                    identityOptions.Role = role;
                }

                return identityOptions;
            });

            services.AddTransient<IMailService, SendMailService>();
            services.AddHttpContextAccessor();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddScoped(sp => new AuthOptions
            {
                Audience = Configuration["AuthSettings:Audience"],
                Issuer = Configuration["AuthSettings:Issuer"],
                Key = Configuration["AuthSettings:Key"]
            });

            services.Configure<IISOptions>(options => {
                
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
                              UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var dataSeeding = new UsersSeeding(userManager, roleManager);
            dataSeeding.SeedData().Wait();

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseHangfireDashboard();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
