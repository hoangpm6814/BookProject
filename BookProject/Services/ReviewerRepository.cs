using BookProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookProject.Services
{
    public class ReviewerRepository : IReviewerRepository
    {
        private BookDbContext _reviewerContext;

        public ReviewerRepository(BookDbContext reviewerContext)
        {
            _reviewerContext = reviewerContext;
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return _reviewerContext.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefault();
        }

        public Reviewer GetReviewerOfAReview(int reviewId)
        {
            //return _reviewerContext.Reviews.Where(r => r.Id == reviewId).Select(r => r.Reviewer).FirstOrDefault(); // My way: not sure
            var reviewerId = _reviewerContext.Reviews.Where(r => r.Id == reviewId).Select(r => r.Reviewer.Id).FirstOrDefault();
            return _reviewerContext.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _reviewerContext.Reviewers.OrderBy(r => r.LastName).ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return _reviewerContext.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return _reviewerContext.Reviewers.Any(r => r.Id == reviewerId);
        }

        public bool Save()
        {
            var saved = _reviewerContext.SaveChanges(); // Save, update, delete hasn't been executed in DB until this method's called
            return saved >= 0 ? true : false; // If saved > 0: something has changed, 0: nothing's changed, <0: something wrong happenned
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _reviewerContext.Update(reviewer);
            return Save();
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _reviewerContext.Add(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _reviewerContext.Remove(reviewer);
            return Save();
        }
    }
}
