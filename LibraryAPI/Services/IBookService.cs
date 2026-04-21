using LibraryBookBorrowingSystem.DTOs.Books;

namespace LibraryBookBorrowingSystem.Services.Interfaces;

public interface IBookService
{
    Task<List<BookResponseDto>> GetAllAsync();
    Task<BookResponseDto> GetByIdAsync(int id);
    Task<BookResponseDto> CreateAsync(CreateBookRequestDto dto);
    Task<BookResponseDto> UpdateAsync(int id, UpdateBookRequestDto dto);
    Task DeleteAsync(int id);
}
