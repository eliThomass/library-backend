using LibraryAPI.DTOs;
using LibraryAPI.Models;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services;

public class MemberService : IMemberService {
    private readonly IMemberRepository _repository;

    public MemberService(IMemberRepository repository) {
        _repository = repository;
    }

    public async Task<IEnumerable<MemberResponseDto>> GetAllMembersAsync() {
        var members = await _repository.GetAllAsync();
        return members.Select(MapToResponseDto);
    }

    public async Task<MemberResponseDto?> GetMemberByIdAsync(Guid id) {
        var member = await _repository.GetByIdAsync(id);
        return member == null ? null : MapToResponseDto(member);
    }

    public async Task<MemberResponseDto> CreateMemberAsync(MemberRequestDto dto) {
        var newMember = new Member {
            Id = Guid.NewGuid(),
            FullName = dto.FullName,
            Email = dto.Email,
            MembershipDate = DateTime.UtcNow
        };

        var createdMember = await _repository.AddAsync(newMember);
        return MapToResponseDto(createdMember);
    }

    public async Task<MemberResponseDto?> UpdateMemberAsync(Guid id, MemberRequestDto dto) {
        var existingMember = await _repository.GetByIdAsync(id);
        if (existingMember == null) return null;

        existingMember.FullName = dto.FullName;
        existingMember.Email = dto.Email;
        
        var updatedMember = await _repository.UpdateAsync(existingMember);
        return MapToResponseDto(updatedMember);
    }

    public async Task<bool> DeleteMemberAsync(Guid id) {
        return await _repository.DeleteAsync(id);
    }

    private static MemberResponseDto MapToResponseDto(Member member) {
        return new MemberResponseDto {
            Id = member.Id,
            FullName = member.FullName,
            Email = member.Email,
            MembershipDate = member.MembershipDate
        };
    }
}
