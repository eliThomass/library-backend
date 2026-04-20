using LibraryAPI.Models;

namespace LibraryAPI.Repositories;

public interface IMemberRepository {
    Task<Member?> GetByIdAsync(Guid id);
    Task<Member> AddAsync(Member mb);
    Task<IEnumerable<Member>> GetAllAsync();
    Task<Member> UpdateAsync(Member mb);
    Task<bool> DeleteAsync(Guid id);
}
