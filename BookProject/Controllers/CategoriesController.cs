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
        [HttpGet("{categoryId}", Name = "GetCategory")]
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

        //api/categories
        [HttpPost]
        public IActionResult CreateCategory([FromBody] Category CategoryNeedToCreate)
        {
            if (CategoryNeedToCreate == null)
                return BadRequest(ModelState);

            // Check whether Category's name exists in DB or not. 
            // Should be done in Repo, not here.
            var cate = _categoryRepository.GetCategories()
                .Where(c => c.Name.Trim().ToUpper() == CategoryNeedToCreate.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (cate != null)
            {
                ModelState.AddModelError("", $"Category {CategoryNeedToCreate.Name} already exists.");
                return StatusCode(442, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.CreateCategory(CategoryNeedToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong creating {CategoryNeedToCreate.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategory", new { categoryId = CategoryNeedToCreate.Id }, CategoryNeedToCreate);
        }

        //api/countries/categoryId
        [HttpPut("{categoryId}")]
        public IActionResult UpdateCategory(int categoryId, [FromBody] Category CategoryNeedToUpdate)
        {
            if (CategoryNeedToUpdate == null)
                return BadRequest(ModelState);

            if (CategoryNeedToUpdate.Id != categoryId)
                return BadRequest(ModelState);

            if (_categoryRepository.IsDuplicateCategoryName(categoryId, CategoryNeedToUpdate.Name))
            {
                ModelState.AddModelError("", $"Category {CategoryNeedToUpdate.Name} already exists.");
                return StatusCode(442, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.UpdateCategory(CategoryNeedToUpdate))
            {
                ModelState.AddModelError("", $"Something went wrong updating {CategoryNeedToUpdate.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //api/countries/categoryId
        [HttpDelete("{categoryId}")]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var CategoryToDelete = _categoryRepository.GetCategory(categoryId);

            if (_categoryRepository.GetBooksForCategory(categoryId).Count() > 0)
            {
                ModelState.AddModelError("", $"{CategoryToDelete.Name} cannot be deleted because it is used by at least one book.");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.DeleteCategory(CategoryToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting {CategoryToDelete.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
