using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;

    public BookRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books.AsNoTracking().ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books.FindAsync(id);
    }

    public async Task<Book> AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return false;

        bool isCurrentlyBorrowed = await _context.BorrowRecords
            .AnyAsync(r => r.BookId == id && r.Status == BorrowStatus.Borrowed);

        if (isCurrentlyBorrowed)
        {
            throw new InvalidOperationException("Cannot delete this book because it is currently checked out by a member.");
        }

        var historicalRecords = await _context.BorrowRecords
            .Where(r => r.BookId == id)
            .ToListAsync();

        _context.BorrowRecords.RemoveRange(historicalRecords);

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        
        return true;
    }
}
