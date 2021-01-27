using BookProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookProject.Services
{
    public class BookRepository : IBookRepository
    {
        private BookDbContext _bookContext;

        public BookRepository(BookDbContext bookContext)
        {
            _bookContext = bookContext;
        }

        public bool BookExists(int bookId)
        {
            return _bookContext.Books.Any(b => b.Id == bookId);
        }

        public bool BookExists(string bookIsbn)
        {
            return _bookContext.Books.Any(b => b.Isbn == bookIsbn);
        }

        public Book GetBook(int bookId)
        {
            return _bookContext.Books.Where(b => b.Id == bookId).FirstOrDefault();
        }

        public Book GetBook(string bookIsbn)
        {
            return _bookContext.Books.Where(b => b.Isbn == bookIsbn).FirstOrDefault();
        }

        public decimal GetBookRating(int bookId)
        {
            var ratingsOfBook = _bookContext.Reviews.Where(r => r.Book.Id == bookId).Select(r => r.Rating);
            if (ratingsOfBook.Count() <= 0)
                return 0;
            decimal finalRating = 0;
            foreach(var rating in ratingsOfBook)
            {
                finalRating += rating;
            }
            finalRating = ((decimal)finalRating) / ratingsOfBook.Count();
            //finalRating = (decimal)ratingsOfBook.Average();
            return finalRating;
        }

        public ICollection<Book> GetBooks()
        {
            return _bookContext.Books.OrderBy(b => b.Title).ToList();
        }

        public bool IsDuplicateIsbn(int bookId, string bookIsbn)
        {
            var book = _bookContext.Books.Where(b => b.Isbn.Trim().ToUpper() == bookIsbn.Trim().ToUpper() && b.Id == bookId).FirstOrDefault();

            return book == null ? false : true;
        }
        public bool Save()
        {
            var saved = _bookContext.SaveChanges(); 
            return saved >= 0 ? true : false;
        }

        public bool UpdateBook(List<int> authorsId, List<int> categoriesId, Book book)
        {
            var authors = _bookContext.Authors.Where(a => authorsId.Contains(a.Id)).ToList();
            var categories = _bookContext.Categories.Where(c => categoriesId.Contains(c.Id)).ToList();

            var bookAuthorsToDelete = _bookContext.BookAuthors.Where(b => b.BookId == book.Id);
            var bookCategoriesToDelete = _bookContext.BookCategories.Where(b => b.BookId == book.Id);

            _bookContext.RemoveRange(bookAuthorsToDelete);
            _bookContext.RemoveRange(bookCategoriesToDelete);

            foreach (var author in authors)
            {
                var bookAuthor = new BookAuthor()
                {
                    Author = author,
                    Book = book
                };
                _bookContext.Add(bookAuthor);
            }

            foreach (var category in categories)
            {
                var bookCategory = new BookCategory()
                {
                    Category = category,
                    Book = book
                };
                _bookContext.Add(bookCategory);
            }

            _bookContext.Update(book);

            return Save();
        }

        public bool CreateBook(List<int> authorsId, List<int> categoriesId, Book book)
        {
            var authors = _bookContext.Authors.Where(a => authorsId.Contains(a.Id)).ToList();
            var categories = _bookContext.Categories.Where(c => categoriesId.Contains(c.Id));

            foreach(var author in authors)
            {
                var bookAuthor = new BookAuthor()
                {
                    Author = author,
                    Book = book
                };
                _bookContext.Add(bookAuthor);
            }

            foreach (var category in categories)
            {
                var bookCategory = new BookCategory()
                {
                    Category = category,
                    Book = book
                };
                _bookContext.Add(bookCategory);
            }

            _bookContext.Add(book);

            return Save();
        }

        public bool DeleteBook(Book Book)
        {
            _bookContext.Remove(Book);
            return Save();
        }
    }
}
