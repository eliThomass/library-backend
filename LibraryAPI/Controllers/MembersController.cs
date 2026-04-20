using LibraryAPI.DTOs;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase {
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService) {
        _memberService = memberService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMembers() {
        var members = await _memberService.GetAllMembersAsync();
        return Ok(members); // 200 OK
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMemberById(Guid id) {
        var member = await _memberService.GetMemberByIdAsync(id);
        if (member == null) {
            return NotFound(new { error = $"Member with ID {id} not found." }); // 404 Not Found
        }
        return Ok(member); // 200 OK
    }

    [HttpPost]
    public async Task<IActionResult> CreateMember([FromBody] MemberRequestDto requestDto) {
        // DataAnnotations in DTO automatically trigger 400 Bad Request if invalid
        var createdMember = await _memberService.CreateMemberAsync(requestDto);
        return CreatedAtAction(nameof(GetMemberById), new { id = createdMember.Id }, createdMember); // 201 Created
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateMember(Guid id, [FromBody] MemberRequestDto requestDto) {
        var updatedMember = await _memberService.UpdateMemberAsync(id, requestDto);
        if (updatedMember == null) {
            return NotFound(new { error = $"Member with ID {id} not found." }); // 404 Not Found
        }
        return Ok(updatedMember); // 200 OK
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMember(Guid id) {
        var deleted = await _memberService.DeleteMemberAsync(id);
        if (!deleted) {
            return NotFound(new { error = $"Member with ID {id} not found." }); // 404 Not Found
        }
        return Ok(new { message = "Member deleted successfully." }); // 200 OK
    }
}
