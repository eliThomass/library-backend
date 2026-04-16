using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories;

public class MemberRepository : IMemberRepository {
    private readonly ApplicationDbContext _context;

    public MemberRepository(ApplicationDbContext context) {
        _context = context;
    }

    public IEnumerable<Mumber> Get() {
        return _context.Members.AsNoTracking().ToList();
    }

    public Member? GetById(Guid id) {
        return _context.Members.AsNoTracking().FirstOrDefault(e => e.Id == id);
    }
    
    public Member Add(Member mb) {
        _context.Members.Add(mb);
        _context.SaveChanges();
        return ev;
    }

    public Member Update(Member mb) {
        
    }

    public Member Delete(Member mb) {

    }
}
