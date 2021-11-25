using GenericLoginAspNetMvc.Authorization;
using GenericLoginAspNetMvc.Interfaces;
using GenericLoginAspNetMvc.Models;
using GenericLoginAspNetMvc.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static GenericLoginAspNetMvc.Authorization.CustomClaimRequirement;

namespace GenericLoginAspNetMvc
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
            string connection_string = Configuration["GenericLogin:LocalConnectionString"];


            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            
            services.AddDbContext<ApplicationDbContext>(options=>
            {
                options.UseSqlServer(connection_string);
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();



            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/login";
                config.AccessDeniedPath = "/error/unauthorized";
            });

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromDays(7);
            });


            services.AddAuthorization(config =>
            {
                config.AddPolicy("CustomClaimBasedPolicy", policyBuilder =>
                {
                    policyBuilder.RequireCustomClaim("CustomClaim", "CustomValue");
                });
            });

            services.AddScoped<IAuthorizationHandler, CustomClaimRequirementHandler>();

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomClaimsFactory>();
            services.AddScoped<IClaimsTransformation, CustomClaimsTransformer>();

            services.AddScoped<IUserRepository, UserRepository>();
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
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
