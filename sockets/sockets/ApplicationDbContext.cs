using Microsoft.EntityFrameworkCore;

namespace sockets
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=localhost;Port=3306;Database=Books;Uid=root;Pwd=batata;");
        }
    }
}