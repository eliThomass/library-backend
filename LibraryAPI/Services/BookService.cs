using LibraryAPI.DTOs;
using LibraryAPI.Models;
using LibraryAPI.Repositories;
using LibraryAPI.Services;

namespace LibraryAPI.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<List<BookResponseDto>> GetAllAsync()
    {
        var books = await _bookRepository.GetAllAsync();
        return books.Select(MapToResponseDto).ToList();  //ToListAsyn()
    }

    public async Task<BookResponseDto> GetByIdAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book is null)
            throw new KeyNotFoundException("Book not found.");

        return MapToResponseDto(book);
    }

    public async Task<BookResponseDto> CreateAsync(CreateBookRequestDto dto)
    {
        ValidateBookData(dto.Title, dto.Author, dto.ISBN, dto.TotalCopies, dto.AvailableCopies);

        var existingBook = await _bookRepository.GetByIsbnAsync(dto.ISBN.Trim());
        if (existingBook is not null)
            throw new InvalidOperationException("A book with this ISBN already exists.");

        var book = new Book
        {
            Title = dto.Title.Trim(),
            Author = dto.Author.Trim(),
            ISBN = dto.ISBN.Trim(),
            TotalCopies = dto.TotalCopies,
            AvailableCopies = dto.AvailableCopies
        };

        await _bookRepository.AddAsync(book);

        return MapToResponseDto(book);
    }

    public async Task<BookResponseDto> UpdateAsync(int id, UpdateBookRequestDto dto)
    {
        ValidateBookData(dto.Title, dto.Author, dto.ISBN, dto.TotalCopies, dto.AvailableCopies);

        var existingBook = await _bookRepository.GetByIdAsync(id);
        if (existingBook is null)
            throw new KeyNotFoundException("Book not found.");

        var isbnOwner = await _bookRepository.GetByIsbnAsync(dto.ISBN.Trim());
        if (isbnOwner is not null && isbnOwner.Id != id)
            throw new InvalidOperationException("A book with this ISBN already exists.");

        existingBook.Title = dto.Title.Trim();
        existingBook.Author = dto.Author.Trim();
        existingBook.ISBN = dto.ISBN.Trim();
        existingBook.TotalCopies = dto.TotalCopies;
        existingBook.AvailableCopies = dto.AvailableCopies;

        await _bookRepository.UpdateAsync(existingBook);

        return MapToResponseDto(existingBook);
    }

    public async Task DeleteAsync(int id)
    {
        var existingBook = await _bookRepository.GetByIdAsync(id);
        if (existingBook is null)
            throw new KeyNotFoundException("Book not found.");

        await _bookRepository.DeleteAsync(existingBook);
    }

    
    private static void ValidateBookData(
        string title,
        string author,
        string isbn,
        int totalCopies,
        int availableCopies)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.");

        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Author is required.");

        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN is required.");

        if (totalCopies <= 0)
            throw new ArgumentException("TotalCopies must be greater than 0.");

        if (availableCopies < 0)
            throw new ArgumentException("AvailableCopies must be >= 0.");

        if (availableCopies > totalCopies)
            throw new ArgumentException("AvailableCopies cannot exceed TotalCopies.");
    }

    
    private static BookResponseDto MapToResponseDto(Book book)
    {
        return new BookResponseDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            ISBN = book.ISBN,
            TotalCopies = book.TotalCopies,
            AvailableCopies = book.AvailableCopies
        };
    }
}
