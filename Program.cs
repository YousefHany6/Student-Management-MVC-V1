using FStudentManagement.Data;
using FStudentManagement.Models;
using FStudentManagement.Rpo_models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FStudentManagement
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
			builder.Services.Configure<RequestLocalizationOptions>(options =>
			{
				options.DefaultRequestCulture = new RequestCulture("ar"); // Set default culture to Arabic
				options.SupportedCultures = new List<CultureInfo> { new CultureInfo("ar") }; // Supported cultures
				options.SupportedUICultures = new List<CultureInfo> { new CultureInfo("ar") }; // Supported UI cultures
			});
			builder.Services.AddIdentity<SUser, IdentityRole>()
							.AddEntityFrameworkStores<ApplicationDbContext>()
							.AddDefaultUI()
							.AddDefaultTokenProviders();
			builder.Services.AddControllersWithViews();
			builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


			builder.Services.Configure<IdentityOptions>(options =>
				{
					options.Password.RequiredLength = 8; // Minimum password length
					options.Password.RequireDigit = false; // Require at least one digit
					options.Password.RequireLowercase = false; // Require at least one lowercase letter
					options.Password.RequireUppercase = false; // Require at least one uppercase letter
					options.Password.RequireNonAlphanumeric = false; // Require at least one special character

					// Define a custom password pattern using a regular expression
					options.Password.RequiredUniqueChars = 0; // Number of unique characters in the password
					options.Password.RequiredUniqueChars = 0; // Number of unique characters in the password
					options.Password.RequiredUniqueChars = 0; // Number of unique characters in the password
				});


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

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
							name: "default",
							pattern: "{controller=Manage}/{action=Index}/{id?}");

			app.MapRazorPages();


			app.Run();
		}
	}
}
