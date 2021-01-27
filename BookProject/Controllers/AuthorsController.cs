using BookProject.DTOs;
using BookProject.Models;
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
    public class AuthorsController : Controller
    {
        private IAuthorRepository _authorRepository;
        private IBookRepository _bookRepository;

        public AuthorsController(IAuthorRepository authorRepository, IBookRepository bookRepository)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
        }

        //api/authors
        [HttpGet]
        public IActionResult GetAuthors()
        {
            var authors = _authorRepository.GetAuthors().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorsDTO = new List<AuthorDTO>();
            foreach (var author in authors)
            {
                authorsDTO.Add(new AuthorDTO
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                });
            }
            return Ok(authorsDTO);
        }

        //api/authors/authorId
        [HttpGet("{authorId}", Name = "GetAuthor")]
        public IActionResult GetAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();

            var author = _authorRepository.GetAuthor(authorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorDTO = new AuthorDTO()
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName
            };
            return Ok(authorDTO);
        }

        //api/authors/books/bookId
        [HttpGet("books/{bookId}")]
        public IActionResult GetAuthorsOfABook(int bookId)
        {
            // Check book with Id exists
            if (!_bookRepository.BookExists(bookId))
                return NotFound();

            var authors = _authorRepository.GetAuthorsOfABook(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorsDTO = new List<AuthorDTO>();
            foreach (var author in authors)
            {
                authorsDTO.Add(new AuthorDTO
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                });
            }
            return Ok(authorsDTO);
        }

        //api/authors/authorId/books
        [HttpGet("{authorId}/books")]
        public IActionResult GetBooksByAuthor(int authorId)
        {
            // Check author with Id exists
            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();

            var books = _authorRepository.GetBooksByAuthor(authorId);

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

        //api/countries
        [HttpPost]
        public IActionResult CreateAuthor([FromBody] Author authorNeedToCreate)
        {
            if (authorNeedToCreate == null)
                return BadRequest(ModelState);

            // Check whether Author's name exists in DB or not. 
            // Should be done in Repo, not here.
            var Author = _authorRepository.GetAuthors()
                .Where(c => c.FirstName.Trim().ToUpper() == authorNeedToCreate.FirstName.Trim().ToUpper()
                && c.LastName.Trim().ToUpper() == authorNeedToCreate.LastName.Trim().ToUpper())
                .FirstOrDefault();

            if (Author != null)
            {
                ModelState.AddModelError("", $"Author {authorNeedToCreate.FirstName} {authorNeedToCreate.LastName} already exists.");
                return StatusCode(442, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_authorRepository.CreateAuthor(authorNeedToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong creating {authorNeedToCreate.FirstName}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetAuthor", new { authorId = authorNeedToCreate.Id }, authorNeedToCreate);
        }

        //api/countries/authorId
        [HttpPut("{authorId}")]
        public IActionResult UpdateAuthor(int authorId, [FromBody] Author authorNeedToUpdate)
        {
            if (authorNeedToUpdate == null)
                return BadRequest(ModelState);

            if (authorNeedToUpdate.Id != authorId)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_authorRepository.UpdateAuthor(authorNeedToUpdate))
            {
                ModelState.AddModelError("", $"Something went wrong updating {authorNeedToUpdate.FirstName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //api/countries/authorId
        [HttpDelete("{authorId}")]
        public IActionResult DeleteAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();

            var AuthorToDelete = _authorRepository.GetAuthor(authorId);

            if (_authorRepository.GetBooksByAuthor(authorId).Count() > 0)
            {
                ModelState.AddModelError("", $"{AuthorToDelete.FirstName} {AuthorToDelete.LastName}" +
                    $"cannot be deleted because it is used by at least one author.");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_authorRepository.DeleteAuthor(AuthorToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting {AuthorToDelete.FirstName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
