using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs;

public class MemberRequestDto {
    [Required(ErrorMessage = "FullName is required.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email must be a valid email address.")]
    public string Email { get; set; } = string.Empty;
}
