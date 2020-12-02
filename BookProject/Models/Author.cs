using System;
using System.Collections.Generic;

namespace BookProject.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }

    }
}
