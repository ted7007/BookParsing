using BookParserAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace BookParserAPI.Repository;

public class ApplicationContext : DbContext
{
    public DbSet<Models.Book> Books { get; set; }

    public DbSet<Models.Tag> Tags { get; set; }

    public DbSet<Models.ISBN> ISBNs { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Models.Book>()
            .HasMany(b => b.Tags)
            .WithMany(t => t.Books);

        modelBuilder
            .Entity<Models.Tag>()
            .HasIndex(t => t.Name)
            .IsUnique();
        
        base.OnModelCreating(modelBuilder);
    }
}