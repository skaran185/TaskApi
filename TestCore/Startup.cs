using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Localdb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using TestCore;
using Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.SpaServices.AngularCli;

namespace Api
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

            services.AddOptions();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services
                .AddDbContextPool<StudentContext>( // replace "YourDbContext" with the class name of your DbContext
                options => options.UseMySql(Configuration.GetConnectionString("StudentEntities"), // replace with your Connection String
                    mySqlOptions =>
                    {
                        mySqlOptions.ServerVersion(new Version(5, 7, 17), ServerType.MySql); // replace with your Server Version and Type
                    }

            ));

            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().Build();
                });
            });
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(Options =>
            {
                Options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                var resolver = Options.SerializerSettings.ContractResolver;
                if (resolver != null)
                    (resolver as DefaultContractResolver)
                        .NamingStrategy = null;
            });
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseExceptionHandler(errorApp =>
            //{
            //    errorApp.Run(async context =>
            //     {
            //         var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
            //         var exception = errorFeature.Error;

            //         // log the exception etc..
            //         // produce some response for the caller
            //     });
            //});

            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseCors();
            app.UseCors("EnableCORS");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }


    }
}