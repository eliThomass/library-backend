using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories;

public class MemberRepository : IMemberRepository {
    private readonly ApplicationDbContext _context;

    public MemberRepository(ApplicationDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<Member>> GetAllAsync() {
        return await _context.Members.AsNoTracking().ToListAsync();
    }

    public async Task<Member?> GetByIdAsync(Guid id) {
        return await _context.Members.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Member> AddAsync(Member mb) {
        await _context.Members.AddAsync(mb);
        await _context.SaveChangesAsync();
        return mb;
    }

    public async Task<Member> UpdateAsync(Member mb) {
        _context.Members.Update(mb);
        await _context.SaveChangesAsync();
        return mb;
    }

    public async Task<bool> DeleteAsync(Guid id) {
        var mb = await _context.Members.FindAsync(id); //FinAsync()
        if (mb == null) return false;

        // Check for active borrows
        bool hasActiveBorrows = await _context.BorrowRecords
            .AnyAsync(r => r.MemberId == id && r.Status == BorrowStatus.Borrowed);

        if (hasActiveBorrows)
        {
            throw new InvalidOperationException("Cannot delete this member because they currently have unreturned books.");
        }

       var historicalRecords = await _context.BorrowRecords
            .Where(r => r.MemberId == id)
            .ToListAsync();
            
        _context.BorrowRecords.RemoveRange(historicalRecords);

        // Now we can delete, after removing borrow records
        _context.Members.Remove(mb);
        await _context.SaveChangesAsync();
        return true;
    }
}
