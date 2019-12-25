using AutoMapper;
using WorkpulseApp.Extensions;
using WorkpulseApp.Filters;
using WorkpulseApp.Helpers;
using WorkpulseApp.Models;
using WorkpulseApp.Repository;
using WorkpulseApp.Repository.UserAdmin;
using WorkpulseApp.Service.PDF;
using WorkpulseApp.Service.UserAdmin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
//using WorkpulseApp.Helpers;

namespace WorkpulseApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public const string ObjectIdentifierType = "http://schemas.microsoft.com/identity/claims/objectidentifier";
        public const string TenantIdType = "http://schemas.microsoft.com/identity/claims/tenantid";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(config =>
            {
                // clear out default configuration
                config.ClearProviders();

                config.AddFilter("Microsoft", LogLevel.None)
               .AddFilter("System", LogLevel.None)
               .AddConsole();

            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.API_Secret);

            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddAzureAd(options => Configuration.Bind("AzureAd", options))
            .AddCookie(cfg => cfg.SlidingExpiration = true)            
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //Automapper
            services.AddAutoMapper(typeof(Startup));

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.Filters.Add(new APIExceptionFilterAttribute());
                options.AllowEmptyInputInBodyModelBinding = true;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddControllersAsServices();

            services.AddSession();

            // Add application services.
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<IStampService, StampService>();
            services.AddTransient<IUserAdminService, UserAdminService>();
            services.AddTransient<IUserAdminRepository, UserAdminRepository>();
            services.AddTransient<CommonHelper>();
            // services.AddSingleton<IGraphAuthProvider, GraphAuthProvider>();
            // services.AddTransient<IGraphSdkHelper, GraphSdkHelper>();

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options =>
                options.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials());
                c.AddPolicy("AllowOrigin", options => options.WithOrigins("http://localhost:44363"));
            });

            services.Configure<BlobConfig>(Configuration.GetSection("BlobConfig"));

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist/ClientApp";
            });

            var connection = @"data source=WORKPULSE\MSSQLSERVER2;initial catalog=CORTNEDB;user id=sa;password=Sa123!@#;multipleactiveresultsets=True;application name=EntityFramework";
            services.AddDbContext<CORTNEDEVContext>(options => options.UseSqlServer(connection));

            var connection2 = @"Server=WORKPULSE\MSSQLSERVER2;Database=CORTNEDB;Trusted_Connection=True;";
            services.AddDbContext<CORTNEDBContext>(options => options.UseSqlServer(connection2));

            services.AddAutoMapper(typeof(Startup));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddLog4Net();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();
            app.UseCors(options => options
            .AllowAnyOrigin()
            .WithOrigins("http://localhost:44363")
            .AllowAnyHeader());
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";
                spa.Options.StartupTimeout = new TimeSpan(0, 5, 0);

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
