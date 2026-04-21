using LibraryAPI.Models;

namespace LibraryAPI.Repositories;

public interface IBorrowRepository
{
    Task<IEnumerable<BorrowRecord>> GetAllAsync();
    Task<IEnumerable<BorrowRecord>> GetByMemberIdAsync(Guid memberId);
    Task<BorrowRecord?> GetActiveBorrowAsync(int bookId, Guid memberId);
    Task<BorrowRecord> AddAsync(BorrowRecord record);
    Task<BorrowRecord> UpdateAsync(BorrowRecord record);
}