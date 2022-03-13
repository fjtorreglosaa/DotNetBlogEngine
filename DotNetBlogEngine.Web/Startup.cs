using BlogEngine.Business.AuthServices;
using BlogEngine.Business.AuthServices.Contracts;
using BlogEngine.Business.DomainServices;
using BlogEngine.Business.DomainServices.Interfaces;
using BlogEngine.Business.ValidationServices;
using BlogEngine.Business.ValidationServices.Author;
using BlogEngine.Business.ValidationServices.Contracts;
using BlogEngine.Business.ValidationServices.Post;
using BlogEngine.Data.Context;
using BlogEngine.Data.Identity;
using BlogEngine.Data.Repositories;
using BlogEngine.Data.Repositories.Interfaces;
using BlogEngine.Data.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetBlogEngine.Web
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

            services.AddControllers();

            services.AddDbContext<AppIdentityDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("LocalServer"),
                b => b.MigrationsAssembly("BlogEngine.Data")));

            services.AddDbContext<BlogEngineDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LocalServer"),
                b => b.MigrationsAssembly("BlogEngine.Data")));

            //Identity
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;

                // times that allows type the Password.
                options.Lockout.MaxFailedAccessAttempts = 20;
                // User will be blocked for 10 minutes.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            });

            services.ConfigureApplicationCookie(option =>
            {
                // it will remember me by 10 hours. Signin by 10 hours.
                option.ExpireTimeSpan = TimeSpan.FromHours(10);

                // returns Forbidden when User is non authorized.
                option.Events.OnRedirectToAccessDenied = c =>
                {
                    c.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.FromResult(0);
                };

                option.Events.OnRedirectToLogin = c =>
                {
                    c.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.FromResult(0);
                };
            });

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IAuthorDomainService, AuthorDomainService>();
            services.AddTransient<IPostDomainService, PostDomainService>();
            services.AddTransient<IAuthorValidationService, AuthorValidationService>();
            services.AddTransient<IPostValidationService, PostValidationService>();

            //identity
            services.AddTransient<IRoleAuthService, RoleAuthService>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IUserRepository, UserRepository>();          
            services.AddTransient<IUserValitadionService, UserValidationService>();
            services.AddTransient<IUserAuthService, UserAuthService>();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DotNetBlogEngine.Web", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotNetBlogEngine.Web v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
