using Microsoft.EntityFrameworkCore;

namespace ArbitraryApp.Domain;

public class ApplicationDbContext : DbContext
{
#pragma warning disable CS8618 // DbSet Properties are initialized on creation
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
#pragma warning restore CS8618

    public DbSet<ArbitraryRecord> ArbitraryRecords { get; set; }
}