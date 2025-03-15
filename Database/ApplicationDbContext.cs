using Microsoft.EntityFrameworkCore;

namespace Domain.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) => Database.EnsureCreated();
    }
}
