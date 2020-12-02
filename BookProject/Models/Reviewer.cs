using System;
using System.Collections.Generic;

namespace BookProject.Models
{
    public class Reviewer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
