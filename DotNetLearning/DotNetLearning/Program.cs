using DotNetLearning.Application.DBContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace DotNetLearning
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();

            using (var scope = webHost.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var dbContext = serviceProvider.GetRequiredService<LearningDbContext>();
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                SeedUser(dbContext, userManager, roleManager);
            }

            webHost.Run();
        }

        private static void SeedUser(LearningDbContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (dbContext.Roles.Any())
            {
                return;
            }

            var role = AddRoles(dbContext, roleManager);
            dbContext.SaveChangesAsync().Wait();

            AddUsers(dbContext, userManager, role);
            dbContext.SaveChangesAsync().Wait();
        }

        private static void AddUsers(LearningDbContext dbContext, UserManager<IdentityUser> userManager, IdentityRole role)
        {
            var user = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "user@gmail.com",
                UserName = "user@gmail.com",
                PhoneNumber = "0976354547",
                PhoneNumberConfirmed = true,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            userManager.CreateAsync(user, "Aa123456@").Wait();
            userManager.AddToRoleAsync(user, "User").Wait();
        }

        private static IdentityRole AddRoles(LearningDbContext dbContext, RoleManager<IdentityRole> roleManager)
        {
            var roleUser = new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "User",
                NormalizedName = "User"
            };


            dbContext.Roles.Add(roleUser);

            return roleUser;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
