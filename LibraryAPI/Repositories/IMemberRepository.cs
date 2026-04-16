using LibraryAPI.Models;

namespace LibraryAPI.Repositories;

public interface IMemberRepository {
    Member? GetById(Guid id);
    Member Add(Member mb);
    IEnumerable<Member> GetAll();
    Member Update(Member mb);
    Member Delete(Member mb);
}
