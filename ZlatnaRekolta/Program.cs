using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZlatnaRekolta.Data;
using ZlatnaRekolta.Services;

namespace ZlatnaRekolta
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            IMvcBuilder buildMVC = builder.Services.AddControllersWithViews();

            builder.Services.AddControllers(
            options =>
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

            builder.Services.AddRazorPages();

            //PODtiskane na nullAttribute
            builder.Services.AddControllers(
                options =>
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.PrepareDataBase().Wait();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
