using LibraryAPI.DTOs;

namespace LibraryAPI.Services;

public interface IBorrowService
{
    Task<IEnumerable<BorrowResponseDto>> GetAllBorrowsAsync();
    Task<IEnumerable<BorrowResponseDto>> GetBorrowsByMemberAsync(Guid memberId);
    Task<BorrowResponseDto> BorrowBookAsync(BorrowRequestDto dto);
    Task<BorrowResponseDto> ReturnBookAsync(BorrowRequestDto dto);
}