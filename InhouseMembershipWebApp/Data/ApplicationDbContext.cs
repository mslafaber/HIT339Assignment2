using System;
using System.Collections.Generic;
using System.Text;
using InhouseMembershipWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InhouseMembershipWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
    }
}
