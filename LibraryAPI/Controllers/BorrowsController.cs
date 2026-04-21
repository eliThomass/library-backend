// BorrowsController.cs

using LibraryAPI.DTOs;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BorrowsController : ControllerBase
{
    private readonly IBorrowService _borrowService;

    public BorrowsController(IBorrowService borrowService)
    {
        _borrowService = borrowService;
    }

    // GET /api/borrows
    [HttpGet]
    public async Task<IActionResult> GetAllBorrows()
    {
        var records = await _borrowService.GetAllBorrowsAsync();
        return Ok(records);
    }

    // GET /api/borrows/member/{memberId}
    [HttpGet("member/{memberId:guid}")]
    public async Task<IActionResult> GetBorrowsByMember(Guid memberId)
    {
        try
        {
            var records = await _borrowService.GetBorrowsByMemberAsync(memberId);
            return Ok(records);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    // POST /api/borrows
    [HttpPost]
    public async Task<IActionResult> BorrowBook([FromBody] BorrowRequestDto dto)
    {
        try
        {
            var record = await _borrowService.BorrowBookAsync(dto);
            return CreatedAtAction(nameof(GetAllBorrows), new { }, record);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });          // 404
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });          // 409
        }
    }

    // PUT /api/borrows/return
    [HttpPut("return")]
    public async Task<IActionResult> ReturnBook([FromBody] BorrowRequestDto dto)
    {
        try
        {
            var record = await _borrowService.ReturnBookAsync(dto);
            return Ok(record);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });          // 404
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });          // 409
        }
    }
}