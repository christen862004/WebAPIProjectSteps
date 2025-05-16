using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebAPIProject.Models
{
    public class CompanyContext:IdentityDbContext<ApplicationUser>
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public CompanyContext()
        {
            
        }
        public CompanyContext(DbContextOptions<CompanyContext> options):base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=WebAPISteps;Integrated Security=True;Encrypt=False");
        }
    }
}
