using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<BorrowRecord> BorrowRecords => Set<BorrowRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Book Configuration
        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(b => b.Title).IsRequired();
            entity.Property(b => b.Author).IsRequired();
            
            entity.Property(b => b.ISBN).IsRequired();
            entity.HasIndex(b => b.ISBN).IsUnique();
        });

        // Member Configuration
        modelBuilder.Entity<Member>(entity =>
        {
            entity.Property(m => m.FullName).IsRequired();
            entity.Property(m => m.Email).IsRequired();
            
            entity.HasIndex(m => m.Email).IsUnique();
        });

        // BorrowRecord Configuration (relationships)
        modelBuilder.Entity<BorrowRecord>(entity =>
        {
            // Linking BorrowRecord to Book
            entity.HasOne(r => r.Book)
                .WithMany()
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            // Linking BorrowRecord to Member
            entity.HasOne(r => r.Member)
                .WithMany()
                .HasForeignKey(r => r.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
