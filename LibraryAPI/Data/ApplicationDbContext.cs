using LibraryBookBorrowingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryBookBorrowingSystem.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(b => b.Title)
                .IsRequired();

            entity.Property(b => b.Author)
                .IsRequired();

            entity.Property(b => b.ISBN)
                .IsRequired();

            entity.HasIndex(b => b.ISBN)
                .IsUnique();
        });
    }
}
