using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TKMS.Service.Ioc;
using Core.Utility.Extensions;
using Microsoft.Extensions.FileProviders;
using System.IO;
using TKMS.Abstraction.ComplexModels;
using Serilog.Context;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Newtonsoft.Json;

namespace TKMS.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o =>
                {
                    o.LoginPath = "/Account";
                    o.SlidingExpiration = true;
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(Convert.ToInt32(Configuration.GetSection("ExpireTimeSpanMinutes").Value));
                });
            services.AddSession();
            services.RegisterServices(Configuration);

            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            services.Configure<BfilAuthSettings>(Configuration.GetSection("BfilAuth"));
            services.Configure<BfilSmsSettings>(Configuration.GetSection("BfilSMS"));
            services.Configure<IWorksSettings>(Configuration.GetSection("IWorksConfig"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            var dateformat = new DateTimeFormatInfo
            {
                ShortDatePattern = "dd-MM-yyyy",
                LongDatePattern = "dd-MM-yyyy hh:mm:ss tt"
            };
            culture.DateTimeFormat = dateformat;

            var supportedCultures = new[] { culture };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
            });

            CultureInfo.DefaultThreadCurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/NotFound/{0}");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // below code is needed to get User name for Log             
            app.Use(async (httpContext, next) =>
            {
                var userName = httpContext.User.Identity.IsAuthenticated ? httpContext.User.Identity.Name : "Guest"; //Gets user Name from user Identity  
                LogContext.PushProperty("Username", userName); //Push user in LogContext;  
                await next.Invoke();
            }
            );

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Index}/{id?}");
            });
        }
    }
}
