using BookProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookProject.Services
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);
        ICollection<Category> GetCategoriesOfABook(int bookId);
        ICollection<Book> GetBooksForCategory(int categoryId);
        bool CategoryExists(int categoryId);
        bool IsDuplicateCategoryName(int categoryId, string categoryName);

        bool CreateCategory(Category category);
        bool DeleteCategory(Category category);
        bool UpdateCategory(Category category);
        bool Save();
    }
}
