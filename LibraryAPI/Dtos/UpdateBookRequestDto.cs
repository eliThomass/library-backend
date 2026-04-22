using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs; 

public class UpdateBookRequestDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Author { get; set; } = string.Empty;

    [Required]
    public string ISBN { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "TotalCopies must be greater than 0.")]
    public int TotalCopies { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "AvailableCopies must be greater than or equal to 0.")]
    public int AvailableCopies { get; set; }
}
