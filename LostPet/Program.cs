using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;
using LostPet.Components;
using LostPet.Components.Pages.Account;
using LostPet.Data;
using LostPet.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LostPet.Utils;

namespace LostPet
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static bool Testing { get; set; } = false;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<PetService>();
            builder.Services.AddScoped<ReportService>();
            builder.Services.AddScoped<SightingsService>();
            builder.Services.AddScoped<UserIdentityProcessor>();
            builder.Services.AddScoped<FilterService>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
                .AddIdentityCookies();
            string uiTests = args.Length > 0 ? args.First() : "";
            Testing = uiTests == "uiTests";
            var connectionString = builder.Configuration.GetConnectionString(uiTests == "uiTests" ? "UiTestsConnection" : "DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            //builder.Services.AddDbContextFactory<PetDbContext>(opt => opt.UseSqlServer(connectionString));
            //builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            // Add additional endpoints required by the Identity /Account Razor components.
            app.MapAdditionalIdentityEndpoints();

            app.Run();
        }
    }
}
