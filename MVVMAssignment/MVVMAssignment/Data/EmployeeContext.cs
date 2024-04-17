using Microsoft.EntityFrameworkCore;
using MVVMAssignment.Models;

namespace MVVMAssignment.Data
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext() { }
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=MVVMDb;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
            }
        }
    }
}
