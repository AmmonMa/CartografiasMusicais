using CartografiasMusicais.Business.Context;
using CartografiasMusicais.Business.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Slugify;

namespace CartografiasMusicais
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CoreContext>(options => options.UseLazyLoadingProxies().UseMySql(Configuration.GetConnectionString("KingHostCS")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<CoreContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth/SignIn";
                options.AccessDeniedPath = "/Auth/SignIn";
            });

            services.AddSession();
            services.AddTransient<ISlugHelper, SlugHelper>();
            services.AddTransient<ICidadeService, CidadeService>();

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add(new ResponseCacheAttribute() { NoStore = true, Location = ResponseCacheLocation.None });
            })
            .AddNewtonsoftJson(opt => { opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<CoreContext>();
                context.Database.EnsureCreated();
            }
            app.UseForwardedHeaders();
            app.UseDeveloperExceptionPage();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();

                app.UseStatusCodePages();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute("pers_route_0", "admin",
                    defaults: new { area = "Admin", controller = "Cidade", action = "Index" });

                routes.MapRoute("pers_route_1", "apresenta",
                    defaults: new { controller = "Home", action = "Apresentacao" });

                routes.MapRoute("pers_route_2", "equipe",
                    defaults: new { controller = "Home", action = "Equipe" });

                routes.MapRoute("pers_route_3", "contato",
                    defaults: new { controller = "Home", action = "Contato" });

                routes.MapRoute("pers_route_4", "espacos",
                    defaults: new { controller = "EspacosView", action = "Index" });

                routes.MapRoute("pers_route_5", "espacos/{*slug}",
                    defaults: new { controller = "EspacosView", action = "Details" });

                routes.MapRoute("pers_route_6", "musicas",
                    defaults: new { controller = "MusicaView", action = "Index" });

                routes.MapRoute("pers_route_7", "musicas/{*slug}",
                    defaults: new { controller = "MusicaView", action = "Details" });

                routes.MapRoute("pers_route_8", "grupos",
                        defaults: new { controller = "GruposView", action = "Index" });

                routes.MapRoute("pers_route_9", "grupos/{*slug}",
                    defaults: new { controller = "GruposView", action = "Details" });

                routes.MapRoute("pers_route_10", "corpos",
                    defaults: new { controller = "CorposView", action = "Index" });

                routes.MapRoute("pers_route_11", "corpos/{*slug}",
                    defaults: new { controller = "CorposView", action = "Details" });

                routes.MapRoute("pers_route_12", "narrativas",
                    defaults: new { controller = "NarrativaView", action = "Index" });

                routes.MapRoute("pers_route_13", "narrativas/musicos/{*slug}",
                    defaults: new { controller = "NarrativaView", action = "Musico" });

                routes.MapRoute("pers_route_14", "narrativas/frequentadores/{*slug}",
                    defaults: new { controller = "NarrativaView", action = "Frequentador" });

                routes.MapRoute("pers_route_15", "narrativas/vozes/{*slug}",
                     defaults: new { controller = "NarrativaView", action = "Voz" });

                routes.MapRoute("pers_route_16", "cidades/{*slug}",
                    defaults: new { controller = "CidadeView", action = "Details" });

                routes.MapRoute("pers_route_17", "{permalink}/musicas/{slug?}",
                    defaults: new { controller = "MusicaView", action = "Specific" });

                routes.MapRoute("pers_route_18", "{permalink}/narrativas/{slug?}",
                    defaults: new { controller = "NarrativaView", action = "Specific" });

                routes.MapRoute("pers_route_19", "{permalink}/corpos/{slug?}",
                    defaults: new { controller = "CorposView", action = "Specific" });
            });
        }
    }
}
