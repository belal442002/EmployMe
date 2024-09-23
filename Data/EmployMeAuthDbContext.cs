using EmployMe.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Emit;

namespace EmployMe.Data
{
    public class EmployMeAuthDbContext : IdentityDbContext<Account>
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<AvailableJob> jobs { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Application> Applications { get; set; }
        public EmployMeAuthDbContext(DbContextOptions<EmployMeAuthDbContext> options) : base(options)
        {
            
        }     

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Adding Roles To Db
            var adminRoleId = "1f628e43-1666-4ecd-a868-8e81f55a0d4a";
            var companyRoleId = "d8e765c8-d310-48e0-b640-9b6b0de2ee28";
            var employeeRoleId = "005a29dc-6b40-44cd-9a38-c70cd19fc934";
            var roles = new List<IdentityRole>
            {

                new IdentityRole
                {
                    Id=adminRoleId,
                    ConcurrencyStamp=adminRoleId,
                    Name="Admin",
                    NormalizedName="Admin".ToUpper(),
                },
                new IdentityRole
                {
                    Id=companyRoleId,
                    ConcurrencyStamp=companyRoleId,
                    Name="Company",
                    NormalizedName="Company".ToUpper(),
                },
                 new IdentityRole
                {
                    Id=employeeRoleId,
                    ConcurrencyStamp=employeeRoleId,
                    Name="Employee",
                    NormalizedName="Employee".ToUpper(),
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);

            // Application Table
            builder.Entity<Application>()
                .HasKey(a => new { a.JobId, a.EmployeeId });

            builder.Entity<Application>()
                .HasOne(a => a.Job)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Application>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.Applications)
                .HasForeignKey(a => a.EmployeeId)
                //.OnDelete(DeleteBehavior.Cascade);
                .OnDelete(DeleteBehavior.Restrict); // Change to Restrict
        }
    }
}
