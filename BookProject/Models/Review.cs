using System;
namespace BookProject.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Headline { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public virtual Reviewer Reviewer { get; set; }
        public virtual Book Book { get; set; }
    }
}
