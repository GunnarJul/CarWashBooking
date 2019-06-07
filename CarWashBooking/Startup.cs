using System;
using CarWashBooking.DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;
using CarWashBooking.Extensions;
using CarWashBooking.Services;
using Microsoft.AspNetCore.Http;

namespace CarWashBooking
{
    public class Startup
    {

        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IConfiguration Configuration { get { return _configuration; } }

        public void ConfigureServices(IServiceCollection services)
        {
            // database connectionstring
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            // entityframe work,
            services.AddEntityFrameworkSqlServer().
            AddDbContext<CarWashBookingDbContext>((serviceProvider, options) =>
            options.UseSqlServer(connectionString).
            UseInternalServiceProvider(serviceProvider));

            // Identity tilføjes
            services.AddDbContext<IdentityDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOpt => sqlOpt.MigrationsAssembly("CarWashBooking"))); // !

            var DbContextOptionBuilder = new DbContextOptionsBuilder<CarWashBookingDbContext>().UseSqlServer(connectionString);
            services.AddSingleton(DbContextOptionBuilder.Options);



            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IdentityDbContext>();
            services.AddAuthentication();

            services.AddSingleton<ICarWashBookingService, CarWashBookingService>();
            services.Configure<EmailServiceOptions>(_configuration.GetSection("Email"));
            services.AddSingleton<IEmailService, EmailService>();

            

            services.AddSession(s => s.IdleTimeout = TimeSpan.FromMinutes(30));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };

            app.UseWebSockets(webSocketOptions);

            app.UseMiddleware<OnLineUsersMiddelWare>();

            app.UseSession();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateUserRoles(services).Wait();

            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<CarWashBookingDbContext>().Database.Migrate();
            }
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("You are not supposed to get here");
            });
        }

        // kode tilpasset fra : https://social.technet.microsoft.com/wiki/contents/articles/51333.asp-net-core-2-0-getting-started-with-identity-and-role-management.aspx#Step_3_Add_Identity_Service_in_Startup_cs
        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            string AdminName = "Admin";
            var adminUser = Configuration.GetSection("SysAdmin:SysAdminUser").Value;
            var adminUserPw = Configuration.GetSection("SysAdmin:SysAdminPassWord").Value;

            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();


            var result = await RoleManager.FindByNameAsync(AdminName);
            if (result == null)
            {
                await RoleManager.CreateAsync(new IdentityRole(AdminName));
            }
            var user = await UserManager.FindByEmailAsync(adminUser);
            if (user == null)
            {
                user = new IdentityUser { Email = adminUser, UserName = adminUser };
                var resultUser = await UserManager.CreateAsync(user, adminUserPw);
                if (resultUser.Succeeded)
                    await UserManager.AddToRoleAsync(user, AdminName);
            }
        }
    }
}
