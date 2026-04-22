using LibraryAPI.DTOs;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    // GET: api/books
    [HttpGet]
    public async Task<ActionResult<List<BookResponseDto>>> GetAll()
    {
        var books = await _bookService.GetAllAsync();
        return Ok(books);
    }

    // GET: api/books/{id}
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

    // POST: api/books
    [HttpPost]
    public async Task<ActionResult<BookResponseDto>> Create([FromBody] CreateBookRequestDto dto)
    {
        // Level 1: Controller validation
        if (!ModelState.IsValid)
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
            // Level 2: Service/Business logic validation
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            // Level 3: Database/Concurrency conflicts
            return Conflict(new { error = ex.Message });
        }
    }

    // PUT: api/books/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<BookResponseDto>> Update(int id, [FromBody] UpdateBookRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { error = "Invalid request data." });
        }

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

    // DELETE: api/books/{id}
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
}
