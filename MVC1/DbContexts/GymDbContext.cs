using GymMangement.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GymMangement.DbContexts
{
    public class GymDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.; Database=Gym; Trusted_Connection=True; TrustServerCertificate=True");
        }

        public DbSet<Plan> Plans { get; set; }
    }
}
