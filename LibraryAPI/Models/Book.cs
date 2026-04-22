using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models;

public class Book
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Author is required")]
    [MaxLength(100)]
    public string Author { get; set; } = string.Empty;

    [Required(ErrorMessage = "ISBN is required")]
    [MaxLength(20)]
    public string ISBN { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Total copies must be at least 1")]
    public int TotalCopies { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Available copies cannot be negative")]
    public int AvailableCopies { get; set; }
}
