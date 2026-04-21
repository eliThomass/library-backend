using LibraryAPI.DTOs;
using LibraryAPI.Models;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services;

public class BorrowService : IBorrowService
{
    private readonly IBorrowRepository _borrowRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IMemberRepository _memberRepository;

    // SemaphoreSlim ensures only one borrow/return runs at a time.
    // This prevents two simultaneous requests from both seeing AvailableCopies > 0
    // and both decrementing, which would push it below zero.
    private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

    public BorrowService(
        IBorrowRepository borrowRepository,
        IBookRepository bookRepository,
        IMemberRepository memberRepository)
    {
        _borrowRepository = borrowRepository;
        _bookRepository = bookRepository;
        _memberRepository = memberRepository;
    }

    public async Task<IEnumerable<BorrowResponseDto>> GetAllBorrowsAsync()
    {
        var records = await _borrowRepository.GetAllAsync();
        return records.Select(MapToDto);
    }

    public async Task<IEnumerable<BorrowResponseDto>> GetBorrowsByMemberAsync(Guid memberId)
    {
        var member = await _memberRepository.GetByIdAsync(memberId);
        if (member == null)
            throw new KeyNotFoundException($"Member with ID {memberId} not found.");

        var records = await _borrowRepository.GetByMemberIdAsync(memberId);
        return records.Select(MapToDto);
    }

    public async Task<BorrowResponseDto> BorrowBookAsync(BorrowRequestDto dto)
    {
        await _lock.WaitAsync();
        try
        {
            var book = await _bookRepository.GetByIdAsync(dto.BookId);
            if (book == null)
                throw new KeyNotFoundException($"Book with ID {dto.BookId} not found.");

            var member = await _memberRepository.GetByIdAsync(dto.MemberId);
            if (member == null)
                throw new KeyNotFoundException($"Member with ID {dto.MemberId} not found.");

            // Re-read AvailableCopies inside the lock to get the latest value
            if (book.AvailableCopies <= 0)
                throw new InvalidOperationException("No available copies of this book.");

            // Optional: prevent borrowing the same book twice without returning
            var existing = await _borrowRepository.GetActiveBorrowAsync(dto.BookId, dto.MemberId);
            if (existing != null)
                throw new InvalidOperationException("Member already has an active borrow for this book.");

            book.AvailableCopies--;
            await _bookRepository.UpdateAsync(book);

            var record = new BorrowRecord
            {
                BookId = dto.BookId,
                MemberId = dto.MemberId,
                BorrowDate = DateTime.UtcNow,
                Status = BorrowStatus.Borrowed
            };

            var created = await _borrowRepository.AddAsync(record);

            // Re-load with navigation properties for the response
            var all = await _borrowRepository.GetAllAsync();
            var full = all.First(r => r.Id == created.Id);
            return MapToDto(full);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<BorrowResponseDto> ReturnBookAsync(BorrowRequestDto dto)
    {
        await _lock.WaitAsync();
        try
        {
            var record = await _borrowRepository.GetActiveBorrowAsync(dto.BookId, dto.MemberId);
            if (record == null)
                throw new InvalidOperationException("No active borrow record found for this member and book.");

            var book = await _bookRepository.GetByIdAsync(dto.BookId);
            if (book == null)
                throw new KeyNotFoundException($"Book with ID {dto.BookId} not found.");

            book.AvailableCopies++;
            await _bookRepository.UpdateAsync(book);

            record.ReturnDate = DateTime.UtcNow;
            record.Status = BorrowStatus.Returned;
            await _borrowRepository.UpdateAsync(record);

            var all = await _borrowRepository.GetAllAsync();
            var full = all.First(r => r.Id == record.Id);
            return MapToDto(full);
        }
        finally
        {
            _lock.Release();
        }
    }

    private static BorrowResponseDto MapToDto(BorrowRecord r) => new BorrowResponseDto
    {
        Id = r.Id,
        BookId = r.BookId,
        BookTitle = r.Book?.Title ?? string.Empty,
        MemberId = r.MemberId,
        MemberName = r.Member?.FullName ?? string.Empty,
        BorrowDate = r.BorrowDate,
        ReturnDate = r.ReturnDate,
        Status = r.Status
    };
}