using BookParserAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace BookParserAPI.Repository;

public class ApplicationContext : DbContext
{
    public DbSet<Models.Book> Products { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}