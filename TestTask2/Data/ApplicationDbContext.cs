using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestTask2.Models;

namespace TestTask2.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<ProjectEmployee> ProjectEmployees { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Company>()
            .HasMany(c => c.CustomerProjects)
            .WithOne(p => p.CustomerCompany)
            .HasForeignKey(p => p.CustomerCompanyId)
            .OnDelete(DeleteBehavior.Restrict); 
        
        builder.Entity<Company>()
            .HasMany(c => c.ExecutorProjects)
            .WithOne(p => p.ExecutorCompany)
            .HasForeignKey(p => p.ExecutorCompanyId)
            .OnDelete(DeleteBehavior.Restrict);
        
        List<IdentityRole<int>> roles = new List<IdentityRole<int>>()
        {
            new IdentityRole<int> {Id = 1, Name = "director", NormalizedName = "DIRECTOR" },
            new IdentityRole<int> {Id = 2, Name = "manager", NormalizedName = "MANAGER" },
            new IdentityRole<int> {Id = 3, Name = "employee", NormalizedName = "EMPLOYEE" },
        };
        builder.Entity<IdentityRole<int>>().HasData(roles);
    }
}