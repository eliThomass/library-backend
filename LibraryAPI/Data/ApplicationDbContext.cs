using LibraryBookBorrowingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryBookBorrowingSystem.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
<<<<<<< borrowing-api
    public DbSet<BorrowRecord> BorrowRecords => Set<BorrowRecord>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BorrowRecord>(entity =>
        {
            entity.HasOne(r => r.Book)
                .WithMany()
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Member)
                .WithMany()
                .HasForeignKey(r => r.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
=======

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
    
    public DbSet<Book> Books { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
>>>>>>> main
}



  
