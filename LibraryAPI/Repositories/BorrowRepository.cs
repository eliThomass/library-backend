using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories;

public class BorrowRepository : IBorrowRepository
{
    private readonly ApplicationDbContext _context;

    public BorrowRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BorrowRecord>> GetAllAsync()
    {
        return await _context.BorrowRecords
            .Include(r => r.Book)
            .Include(r => r.Member)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<BorrowRecord>> GetByMemberIdAsync(Guid memberId)
    {
        return await _context.BorrowRecords
            .Include(r => r.Book)
            .Include(r => r.Member)
            .Where(r => r.MemberId == memberId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<BorrowRecord?> GetActiveBorrowAsync(int bookId, Guid memberId)
    {
        return await _context.BorrowRecords
            .FirstOrDefaultAsync(r =>
                r.BookId == bookId &&
                r.MemberId == memberId &&
                r.Status == BorrowStatus.Borrowed);
    }

    public async Task<BorrowRecord> AddAsync(BorrowRecord record)
    {
        await _context.BorrowRecords.AddAsync(record);
        await _context.SaveChangesAsync();
        return record;
    }

    public async Task<BorrowRecord> UpdateAsync(BorrowRecord record)
    {
        _context.BorrowRecords.Update(record);
        await _context.SaveChangesAsync();
        return record;
    }
}