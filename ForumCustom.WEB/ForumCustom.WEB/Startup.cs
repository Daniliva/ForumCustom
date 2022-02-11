using ForumCustom.BLL;
using ForumCustom.BLL.Contract;
using ForumCustom.BLL.Contract.Manager;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ForumCustom.WEB
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
            string connection = Configuration.GetConnectionString("DefaultConnection");
            IManagerFactory managerFactory = new ManagerFactory(connection);

            services.AddSingleton<IUserManager>(managerFactory.GetManager<IUserManager>());
            services.AddSingleton<IMemberManager>(managerFactory.GetManager<IMemberManager>());
            services.AddSingleton<ITopicManager>(managerFactory.GetManager<ITopicManager>());
            services.AddSingleton<ICommentManager>(managerFactory.GetManager<ICommentManager>());
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                  .AddCookie(options => //CookieAuthenticationOptions
                  {
                      options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/User/Login");
                      options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/User/Login");
                  });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

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
            });
        }
    }
}