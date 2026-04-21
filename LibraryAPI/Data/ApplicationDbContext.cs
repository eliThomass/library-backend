using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Member> Members => Set<Member>();
    public DbSet<Book> Books => Set<Book>();
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
}

