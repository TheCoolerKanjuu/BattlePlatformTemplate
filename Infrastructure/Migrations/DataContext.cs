namespace Infrastructure.Migrations;

using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    /// <inheritdoc/>
    public DataContext(DbContextOptions<DataContext> options) 
        : base(options)
    {
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
        
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }
}