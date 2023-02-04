using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zalandu.Server.Models;
using Zalandu.Server.Models.Interfaces;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Zalandu.Server
{
    public class Startup
    {
        #region -------constructor-------
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        #region -------properties class-------
        public IConfiguration Configuration { get; }
        #endregion
         
        #region -------methods class--------
        public void ConfigureServices(IServiceCollection services)
        {
            #region -------configuration email-client service-------
            services.Configure<EmailServerMAILJET>((options) =>
            {
                options.ServerName = Configuration.GetSection("SMTPMailJet").GetValue<string>("ServerName");
                options.APIKey     = Configuration.GetSection("SMTPMailJet").GetValue<string>("APIKey");
                options.SecretKey  = Configuration.GetSection("SMTPMailJet").GetValue<string>("SecretKey");
            });

            services.AddScoped<IEmailClient, EmailClientMAILJET>();
            #endregion

            #region -------configuration from DBContext of Identity and our own dbcontext: ApplicationDBContext-------
            string _connectionString = Configuration.GetConnectionString("BlazorSqlServerConnectionString");
            string _assembly         = Assembly.GetExecutingAssembly().GetName().Name;

            services.AddDbContext<ApplicationDBContext>((DbContextOptionsBuilder options) => {
                options.UseSqlServer(_connectionString, (SqlServerDbContextOptionsBuilder op) => op.MigrationsAssembly(_assembly));
            });

            services.AddIdentity<IdentityCustomer, IdentityRole>((IdentityOptions options) => {
                options.Password = new PasswordOptions
                {
                    RequireDigit           = true,
                    RequiredLength         = 6,
                    RequireLowercase       = true,
                    RequireUppercase       = true,
                    RequireNonAlphanumeric = true
                };

                options.SignIn = new SignInOptions
                {
                    RequireConfirmedEmail = true
                };

                options.Lockout = new LockoutOptions
                {
                    AllowedForNewUsers      = false,
                    MaxFailedAccessAttempts = 3,
                    DefaultLockoutTimeSpan  = System.TimeSpan.FromHours(5)
                };

                options.User = new UserOptions
                {
                    RequireUniqueEmail = true
                };
            }).AddEntityFrameworkStores<ApplicationDBContext>()
              .AddTokenProvider<DataProtectorTokenProvider<IdentityCustomer>>(TokenOptions.DefaultProvider); //<---to avoid error: .AddTokenProvider<DataProtectorTokenProvider<CustomerUser>>(TokenOptions.DefaultProvider);
            #endregion

            #region -------configuration servicie to generation JWT and authenticate by JWT-------
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer((JwtBearerOptions options) => {
                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                        {
                            ValidateIssuer           = true, 
                            ValidateLifetime         = true,
                            ValidateIssuerSigningKey = true, 
                            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JWT:Key")) )
                        };
                    });
            #endregion

            services.AddControllersWithViews().AddJsonOptions((JsonOptions options) => { options.JsonSerializerOptions.PropertyNamingPolicy = null; });
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            //----definimos uso de Identity para autentificacion/autorizacion----
            app.UseAuthentication();
            app.UseAuthorization();
            //========================

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
        #endregion
    }
}
