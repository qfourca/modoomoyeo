using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using modoomoyeo.Database;
using SignalRChat.Hubs;
//using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Ducademy
{
    public class Startup
    {
        [Obsolete]
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSignalR();
            services.AddControllersWithViews();
            services.Add(new ServiceDescriptor(typeof(UserQurey), new UserQurey(
                Configuration.GetConnectionString("DefaultConnection"))));

            services.Add(new ServiceDescriptor(typeof(ScheduleQurey), new ScheduleQurey(
                Configuration.GetConnectionString("DefaultConnection"))));

            services.Add(new ServiceDescriptor(typeof(ChatingQurey), new ChatingQurey(
                Configuration.GetConnectionString("DefaultConnection"))));


            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/Sign/Signin";
            });

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<ChatHub>("/chathub");
                endpoints.MapHub<ChatHub>("/stream");
                //endpoints.MapHub<AlertHub>("/alerm_signalr");
            });


        }
    }
}
