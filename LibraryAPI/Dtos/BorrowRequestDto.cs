using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs;

public class BorrowRequestDto
{
    [Required]
    public int BookId { get; set; }

    [Required]
    public Guid MemberId { get; set; }
}