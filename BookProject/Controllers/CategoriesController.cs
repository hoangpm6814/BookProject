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

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
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

        // Lack GetBooksForCategory
    }
}
