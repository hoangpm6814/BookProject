using BookProject.DTOs;
using BookProject.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : Controller
    {
        private IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        //api/books
        [HttpGet]
        public IActionResult Getbooks()
        {
            var books = _bookRepository.GetBooks().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var booksDTO = new List<BookDTO>();
            foreach (var book in books)
            {
                booksDTO.Add(new BookDTO
                {
                    Id = book.Id,
                    Title = book.Title,
                    Isbn = book.Isbn,
                    DatePublished = book.DatePublished
                });
            }
            return Ok(booksDTO);
        }

        //api/books/bookId
        [HttpGet("{bookId}")]
        public IActionResult GetBook(int bookId)
        {
            if (!_bookRepository.BookExists(bookId))
                return NotFound();

            var book = _bookRepository.GetBook(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookDTO = new BookDTO()
            {
                Id = book.Id,
                Title = book.Title,
                Isbn = book.Isbn,
                DatePublished = book.DatePublished
            };
            return Ok(bookDTO);
        }

        //api/books/isbn/isbn
        [HttpGet("isbn/{isbn}")]
        public IActionResult GetBookIsbn(string isbn)
        {
            if (!_bookRepository.BookExists(isbn))
                return NotFound();

            var book = _bookRepository.GetBook(isbn);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookDTO = new BookDTO()
            {
                Id = book.Id,
                Title = book.Title,
                Isbn = book.Isbn,
                DatePublished = book.DatePublished
            };
            return Ok(bookDTO);
        }

        //api/books/{bookId}/rating
        [HttpGet("{bookId}/rating")]
        public IActionResult GetBookRating(int bookId)
        {
            if (!_bookRepository.BookExists(bookId))
                return NotFound();
            var rating = _bookRepository.GetBookRating(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rating);
        }
    }
}
