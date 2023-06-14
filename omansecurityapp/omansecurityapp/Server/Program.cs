using System.Diagnostics;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using omansecurityapp.Server.Services;
using omansecurityapp.Shared.Models;

namespace omansecurityapp;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddControllers();
        builder.Services.AddRazorPages();
        builder.Services.AddScoped<IReports, IReports.ReportServices>();
        builder.Services.AddScoped<IUser, UserServices>();

        //datatabase services connection
        //   builder.Services.AddDbContext<ApplicationDbContext>
        //(options =>
        //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        using (var app = builder.Build())
        {

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors(cors => cors
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials()
            );

            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}

