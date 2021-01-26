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
    public class CategoriesController : Controller
    {
        private ICategoryRepository _categoryRepository;
        private IBookRepository _bookRepository;

        public CategoriesController(ICategoryRepository categoryRepository, IBookRepository bookRepository)
        {
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
        }

        //api/categories
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoriesDTO = new List<CategoryDTO>();
            foreach (var category in categories)
            {
                categoriesDTO.Add(new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }
            return Ok(categoriesDTO);
        }

        //api/categories/categoryId
        [HttpGet("{categoryId}")]
        public IActionResult Getcategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var category = _categoryRepository.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryDTO = new CategoryDTO()
            {
                Id = category.Id,
                Name = category.Name
            };
            return Ok(categoryDTO);
        }

        //api/categories/books/bookId
        [HttpGet("books/{bookId}")]
        public IActionResult GetCategoryOfABook(int bookId)
        {
            // Check book with Id exists
            if (!_bookRepository.BookExists(bookId))
                return NotFound();

            var categories = _categoryRepository.GetCategoriesOfABook(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoriesDTO = new List<CategoryDTO>();
            foreach (var category in categories)
            {
                categoriesDTO.Add(new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }

            return Ok(categoriesDTO);
        }

        //api/categories/categoryId/books
        [HttpGet("{categoryId}/books")]
        public IActionResult GetBooksForCategory(int categoryId)
        {
            // Check cate with Id exists
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var books = _categoryRepository.GetBooksForCategory(categoryId);

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
    }
}
