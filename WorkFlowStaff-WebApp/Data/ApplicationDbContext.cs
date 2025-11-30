using Microsoft.EntityFrameworkCore;
using WorkFlowStaff_WebApp.Models;

namespace WorkFlowStaff_WebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Empleado> Empleados { get; set; } = default!;
        public DbSet<Departamento> Departamentos { get; set; } = default!;
        public DbSet<Cargo> Cargos { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
