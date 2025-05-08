using Microsoft.EntityFrameworkCore;

namespace WebAPIProject.Models
{
    public class CompanyContext:DbContext
    {
        public DbSet<Department> Departments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=WebAPISteps;Integrated Security=True;Encrypt=False");
        }
    }
}
