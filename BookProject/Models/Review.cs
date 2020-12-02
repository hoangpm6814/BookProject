using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookProject.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 10, ErrorMessage ="Headline must be between 10 and 200 characters.")]
        public string Headline { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Review must be between 10 and 2000 characters.")]
        public string ReviewText { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage ="Rating must be 1 to 5.")]
        public int Rating { get; set; }
        public virtual Reviewer Reviewer { get; set; }
        public virtual Book Book { get; set; }
    }
}
