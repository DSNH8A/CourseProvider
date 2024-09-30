using CourseProvider.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProvider.Data {
    public class CourseProviderContext : DbContext
    {
        public CourseProviderContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Course> Courses { get; set; } = null!;

        public DbSet<Material> Materials { get; set; } = null!;

        public DbSet<Skill> Skills { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CourseProvider;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
    }
}
