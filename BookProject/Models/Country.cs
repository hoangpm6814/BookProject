using System;
using System.Collections.Generic;

namespace BookProject.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
    }
}
