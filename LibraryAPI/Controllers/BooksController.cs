using LibraryAPI.DTOs;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryBookBorrowingSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult<List<BookResponseDto>>> GetAll()
    {
        var books = await _bookService.GetAllAsync();
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookResponseDto>> GetById(int id)
    {
        try
        {
            var book = await _bookService.GetByIdAsync(id);
            return Ok(book);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

<<<<<<< borrowing-api

        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
=======
    [HttpPost]
    public async Task<ActionResult<BookResponseDto>> Create([FromBody] CreateBookRequestDto dto)
    {
        // level 1: Controller validation
        if (!ModelState.IsValid)
>>>>>>> main
        {
            return BadRequest(new { error = "Invalid request data." });
        }

        try
        {
            var createdBook = await _bookService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdBook.Id }, createdBook);
        }
        catch (ArgumentException ex)
        {
<<<<<<< borrowing-api
            newBook.Id = _books.Max(b => b.Id) + 1;
            _books.Add(newBook);


            _cache.Remove(AllBooksCacheKey);

            return Ok(newBook);
=======     // Level 2: service validation
            return BadRequest(new { error = ex.Message });
>>>>>>> main
        }
        catch (InvalidOperationException ex)
        {
<<<<<<< borrowing-api
            var existing = _books.FirstOrDefault(b => b.Id == id);

            if (existing == null)
                return NotFound();

            existing.Title = updatedBook.Title;
            existing.Author = updatedBook.Author;


            _cache.Remove(AllBooksCacheKey);
            _cache.Remove(BookCacheKey(id));

            return Ok(existing);
=======      // Level 3 Database/Logic conflicts
            return Conflict(new { error = ex.Message });
>>>>>>> main
        }
    }

<<<<<<< borrowing-api

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);

            if (book == null)
                return NotFound();

            _books.Remove(book);


            _cache.Remove(AllBooksCacheKey);
            _cache.Remove(BookCacheKey(id));
=======
    [HttpPut("{id:int}")]
    public async Task<ActionResult<BookResponseDto>> Update(int id, [FromBody] UpdateBookRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { error = "Invalid request data." });
        }
>>>>>>> main

        try
        {
            var updatedBook = await _bookService.UpdateAsync(id, dto);
            return Ok(updatedBook);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

<<<<<<< borrowing-api
=======
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _bookService.DeleteAsync(id);
            return Ok(new { message = "Book deleted successfully." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
>>>>>>> main
}
