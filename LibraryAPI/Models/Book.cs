<<<<<<< borrowing-api
namespace LibraryAPI.Models;
=======
using System.ComponentModel.DataAnnotations;

namespace LibraryBookBorrowingSystem.Models;
>>>>>>> main

public class Book
{
    public int Id { get; set; }
<<<<<<< borrowing-api
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
}
=======

    [Required]
    [MaxLenght(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLenght(20)]
    public string Author { get; set; } = string.Empty;

    [Required]
    [MaxLenght(20)]
    public string ISBN { get; set; } = string.Empty;
    [Range(0, int.MaxValue)]
    public int TotalCopies { get; set; }

    [Range(0, int.MaxValue)]
    public int AvailableCopies { get; set; }
}
>>>>>>> main
