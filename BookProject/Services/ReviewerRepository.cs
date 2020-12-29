using BookProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookProject.Services
{
    public class ReviewerRepository : IReviewerRepository
    {
        public Reviewer GetReviewer(int reviewerId)
        {
            throw new NotImplementedException();
        }

        public Reviewer GetReviewerOfAReview(int reviewId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            throw new NotImplementedException();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            throw new NotImplementedException();
        }

        public bool ReviewerExists(int reviewerId)
        {
            throw new NotImplementedException();
        }
    }
}
