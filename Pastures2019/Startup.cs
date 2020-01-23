using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pastures2019.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pastures2019.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

namespace Pastures2019
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection")));

            services
                    .AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                })
                //.AddIdentity<IdentityUser, IdentityRole>()
                //.AddIdentity<Pastures2019.Models.ApplicationUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.AddIdentityCore<ApplicationUser>()
            //    .AddRoles<IdentityRole>()
            //    .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders()
            //    .AddDefaultUI();

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });

            services.AddMvc(options =>
            {
                var iStrFactory = services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                var L = iStrFactory.Create("ModelBindingMessages", "Pastures2019");

                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor((x) => L["The field is required."]);
                options.ModelBindingMessageProvider.SetValueIsInvalidAccessor((x) => L["The value '{0}' is invalid."]);
                options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor((x) => L["The field {0} must be a number."]);
                options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor((x) => L["A value for the '{0}' property was not provided.", x]);
                options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => L["The value '{0}' is not valid for {1}.", x, y]);
                options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => L["A value is required."]);
                options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor((x) => L["The supplied value is invalid for {0}.", x]);
                options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor((x) => L["Null value is invalid", x]);
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddDataAnnotationsLocalization()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix,
                    options => { options.ResourcesPath = "Resources"; });
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { new CultureInfo("ru"), new CultureInfo("kk"), new CultureInfo("en") };
                options.DefaultRequestCulture = new RequestCulture("ru", "ru");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var supportedCultures = new[]
            {
                new CultureInfo("ru"),
                new CultureInfo("kk"),
                new CultureInfo("en")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateRoles(serviceProvider).Wait();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string[] roleNames = { "Administrator", "Moderator" };
            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            IdentityUser user = userManager.Users.FirstOrDefault(u => u.Email == "n.a.k@bk.ru");
            if (user != null)
            {
                await userManager.AddToRoleAsync(user, "Administrator");
            }
        }
    }
}
