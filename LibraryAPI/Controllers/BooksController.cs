// BooksController.cs


using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using LibraryAPI.Models;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IMemoryCache _cache;

        private static List<Book> _books = new List<Book>
        {
            new Book { Id = 1, Title = "Clean Code", Author = "Robert C. Martin" },
            new Book { Id = 2, Title = "The Pragmatic Programmer", Author = "Andrew Hunt" }
        };

        private const string AllBooksCacheKey = "all_books";

        private string BookCacheKey(int id) => $"book_{id}";

        public BooksController(IMemoryCache cache)
        {
            _cache = cache;
        }
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            if (!_cache.TryGetValue(AllBooksCacheKey, out List<Book> books))
            {
                books = _books;

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(AllBooksCacheKey, books, cacheOptions);
            }

            return Ok(books);
        }


        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            string key = BookCacheKey(id);

            if (!_cache.TryGetValue(key, out Book book))
            {
                book = _books.FirstOrDefault(b => b.Id == id);

                if (book == null)
                    return NotFound();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(key, book, cacheOptions);
            }

            return Ok(book);
        }
        [HttpPost]
        public IActionResult CreateBook(Book newBook)
        {
            newBook.Id = _books.Max(b => b.Id) + 1;
            _books.Add(newBook);


            _cache.Remove(AllBooksCacheKey);

            return Ok(newBook);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, Book updatedBook)
        {
            var existing = _books.FirstOrDefault(b => b.Id == id);

            if (existing == null)
                return NotFound();

            existing.Title = updatedBook.Title;
            existing.Author = updatedBook.Author;


            _cache.Remove(AllBooksCacheKey);
            _cache.Remove(BookCacheKey(id));

            return Ok(existing);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);

            if (book == null)
                return NotFound();

            _books.Remove(book);


            _cache.Remove(AllBooksCacheKey);
            _cache.Remove(BookCacheKey(id));

            return Ok("Book deleted");
        }
    }

}
