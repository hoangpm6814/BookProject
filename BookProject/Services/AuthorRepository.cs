using BookProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookProject.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        private BookDbContext _authorContext;

        public AuthorRepository(BookDbContext authorContext)
        {
            _authorContext = authorContext;
        }

        public bool AuthorExists(int authorId)
        {
            return _authorContext.Authors.Any(a => a.Id == authorId);
        }

        public Author GetAuthor(int authorId)
        {
            return _authorContext.Authors.Where(a => a.Id == authorId).FirstOrDefault();
        }

        public ICollection<Author> GetAuthors()
        {
            return _authorContext.Authors.OrderBy(a => a.FirstName).ToList();
        }

        public ICollection<Author> GetAuthorsOfABook(int bookId)
        {
            return _authorContext.BookAuthors.Where(ba => ba.BookId == bookId).Select(ba => ba.Author).ToList();
        }

        public ICollection<Book> GetBooksByAuthor(int authorId)
        {
            return _authorContext.BookAuthors.Where(ba => ba.AuthorId == authorId).Select(ba => ba.Book).ToList();
        }

        public bool Save()
        {
            var saved = _authorContext.SaveChanges(); // Save, update, delete hasn't been executed in DB until this method's called
            return saved >= 0 ? true : false; // If saved > 0: something has changed, 0: nothing's changed, <0: something wrong happenned
        }

        public bool UpdateAuthor(Author author)
        {
            _authorContext.Update(author);
            return Save();
        }

        public bool CreateAuthor(Author author)
        {
            _authorContext.Add(author);
            return Save();
        }

        public bool DeleteAuthor(Author author)
        {
            _authorContext.Remove(author);
            return Save();
        }
    }
}
