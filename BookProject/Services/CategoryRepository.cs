using BookProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookProject.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private BookDbContext _categoryContext;

        public CategoryRepository(BookDbContext categoryContext)
        {
            _categoryContext = categoryContext;
        }
        public bool CategoryExists(int categoryId)
        {
            return _categoryContext.Categories.Any(c => c.Id == categoryId);
        }

        public ICollection<Book> GetBooksForCategory(int categoryId)
        {
            return _categoryContext.BookCategories.Where(bc => bc.CategoryId == categoryId).Select(b => b.Book).ToList();
        }

        public ICollection<Category> GetCategories()
        {
            return _categoryContext.Categories.OrderBy(c => c.Name).ToList();
        }

        public ICollection<Category> GetCategoriesOfABook(int bookId)
        {

            //var listCategoriesId = _categoryContext.BookCategories.Where(bc => bc.BookId == bookId).Select(c => c.CategoryId).ToList();
            //var listCategories = new List<Category>();

            //foreach(var cateId in listCategoriesId)
            //{
            //    //listCategories.Add(_categoryContext.Categories.Where(c => c.Id == cateId).FirstOrDefault());
            //    listCategories.Add(GetCategory(cateId));
            //}
            //return listCategories;

            return _categoryContext.BookCategories.Where(bc => bc.BookId == bookId).Select(c => c.Category).ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _categoryContext.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
        }

        public bool IsDuplicateCategoryName(int categoryId, string categoryName)
        {
            var category = _categoryContext.Categories.Where(c => c.Name.ToUpper() == categoryName.ToUpper() && c.Id == categoryId).FirstOrDefault();

            return category == null ? false : true;
        }
    }
}
