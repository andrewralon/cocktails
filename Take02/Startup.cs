using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Take02.Models;
using Take02.Import;

namespace Take02
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
            services.AddMvc();

            GetConfig();

            var cocktailsConnectionString = Configuration.GetConnectionString("CocktailsDatabase");
            services.AddDbContext<CocktailsContext>(options => options.UseSqlServer(cocktailsConnectionString));

            // Importer and dependencies
            services.AddTransient<IImporter, Importer>();
            services.AddTransient<IComponentImporter, ComponentImporter>();
            services.AddTransient<ILibraryImporter, LibraryImporter>();
            services.AddTransient<IRawParser, RawParser>();
            services.AddTransient<IRecipeImporter, RecipeImporter>();
            services.AddTransient<IUnitImporter, UnitImporter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public static IConfiguration GetConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json",
                optional: true,
                reloadOnChange: true);

            return builder.Build();
        }
    }
}
