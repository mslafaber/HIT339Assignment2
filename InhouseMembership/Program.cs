using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InhouseMembership.Data;
using InhouseMembership.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InhouseMembership
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            try
            {
                var scope = host.Services.CreateScope();

                var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                ctx.Database.EnsureCreated();

                var adminRole = new IdentityRole("Admin");
                var coachRole = new IdentityRole("Coach");
                var memberRole = new IdentityRole("Member");

                if (!ctx.Roles.Any())
                {
                    roleManager.CreateAsync(adminRole).GetAwaiter().GetResult();
                    roleManager.CreateAsync(coachRole).GetAwaiter().GetResult();
                    roleManager.CreateAsync(memberRole).GetAwaiter().GetResult();

                }

                if (!ctx.Users.Any(u => u.UserName == "Admin"))
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = "Admin",
                        Email = "admin@live.com",
                        PhoneNumber = "0410888999",
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(adminUser, "admin1234").GetAwaiter().GetResult();

                    userManager.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();

                }

                if (!ctx.Users.Any(u => u.UserName == "Coach1"))
                {
                    var coachUser = new ApplicationUser
                    {
                        UserName = "Coach1",
                        Email = "coach1@live.com",
                        PhoneNumber = "04207654321",
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(coachUser, "coach1234").GetAwaiter().GetResult();
                    userManager.AddToRoleAsync(coachUser, coachRole.Name).GetAwaiter().GetResult();
                }
                if (!ctx.Users.Any(u => u.UserName == "Coach2"))
                {
                    var coachUser = new ApplicationUser
                    {
                        UserName = "Coach2",
                        Email = "coach2@live.com",
                        PhoneNumber = "04217654321",
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(coachUser, "coach1234").GetAwaiter().GetResult();
                    userManager.AddToRoleAsync(coachUser, coachRole.Name).GetAwaiter().GetResult();
                }
                


                if (!ctx.Users.Any(u => u.UserName == "Member1"))
                {
                    var member = new ApplicationUser
                    {
                        UserName = "Member1",
                        Email = "member1@live.com",
                        PhoneNumber = "04307654321",
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(member, "member1234").GetAwaiter().GetResult();
                    userManager.AddToRoleAsync(member, memberRole.Name).GetAwaiter().GetResult();
                }
                if (!ctx.Users.Any(u => u.UserName == "Member2"))
                {
                    var member = new ApplicationUser
                    {
                        UserName = "Member2",
                        Email = "member2@live.com",
                        PhoneNumber = "04317654321",
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(member, "member1234").GetAwaiter().GetResult();
                    userManager.AddToRoleAsync(member, memberRole.Name).GetAwaiter().GetResult();
                }
                if (!ctx.Users.Any(u => u.UserName == "Member3"))
                {
                    var member = new ApplicationUser
                    {
                        UserName = "Member3",
                        Email = "member3@live.com",
                        PhoneNumber = "04327654321",
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(member, "member1234").GetAwaiter().GetResult();
                    userManager.AddToRoleAsync(member, memberRole.Name).GetAwaiter().GetResult();
                }
                if (!ctx.Users.Any(u => u.UserName == "Member4"))
                {
                    var member = new ApplicationUser
                    {
                        UserName = "Member4",
                        Email = "member4@live.com",
                        PhoneNumber = "04337654321",
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(member, "member1234").GetAwaiter().GetResult();
                    userManager.AddToRoleAsync(member, memberRole.Name).GetAwaiter().GetResult();
                }
                if (!ctx.Users.Any(u => u.UserName == "Member5"))
                {
                    var member = new ApplicationUser
                    {
                        UserName = "Member5",
                        Email = "member5@live.com",
                        PhoneNumber = "04347654321",
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(member, "member1234").GetAwaiter().GetResult();
                    userManager.AddToRoleAsync(member, memberRole.Name).GetAwaiter().GetResult();
                }

                

            }
            catch (Exception e)
            {
                Console.WriteLine("e.Message: " + e.Message);
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
