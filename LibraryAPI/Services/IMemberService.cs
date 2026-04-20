using LibraryAPI.DTOs;

namespace LibraryAPI.Services;

public interface IMemberService {
    Task<IEnumerable<MemberResponseDto>> GetAllMembersAsync();
    Task<MemberResponseDto?> GetMemberByIdAsync(Guid id);
    Task<MemberResponseDto> CreateMemberAsync(MemberRequestDto dto);
    Task<MemberResponseDto?> UpdateMemberAsync(Guid id, MemberRequestDto dto);
    Task<bool> DeleteMemberAsync(Guid id);
}
