using Microsoft.EntityFrameworkCore;

namespace CodeChallenging.Database
{
    public class HealthCheckDbContext : DbContext
    {
        public HealthCheckDbContext(DbContextOptions<HealthCheckDbContext> options) : base(options) { }
    }
}
