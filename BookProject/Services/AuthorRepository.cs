using BookProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookProject.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        public bool AuthorExists(int authorId)
        {
            throw new NotImplementedException();
        }

        public Author GetAuthor(int authorId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Author> GetAuthors()
        {
            throw new NotImplementedException();
        }

        public ICollection<Author> GetAuthorsOfABook(int bookId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Book> GetBooksByAuthor(int authorId)
        {
            throw new NotImplementedException();
        }
    }
}
