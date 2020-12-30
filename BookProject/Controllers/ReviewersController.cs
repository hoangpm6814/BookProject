using BookProject.DTOs;
using BookProject.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewersController : Controller
    {
        private IReviewerRepository _reviewerRepository;
        private IReviewRepository _reviewRepository;

        public ReviewersController(IReviewerRepository reviewerRepository, IReviewRepository reviewRepository)
        {
            _reviewerRepository = reviewerRepository;
            _reviewRepository = reviewRepository;
        }

        //api/reviewers
        [HttpGet]
        public IActionResult GetReviewers()
        {
            var reviewers = _reviewerRepository.GetReviewers().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewersDTO = new List<ReviewerDTO>();
            foreach (var reviewer in reviewers)
            {
                reviewersDTO.Add(new ReviewerDTO
                {
                    Id = reviewer.Id,
                    FirstName = reviewer.FirstName,
                    Lastname = reviewer.LastName
                });
            }
            return Ok(reviewersDTO);
        }

        //api/reviewers/reviewerId
        [HttpGet("{reviewerId}")]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            var reviewer = _reviewerRepository.GetReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ReviewerDTO = new ReviewerDTO()
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                Lastname = reviewer.LastName
            };
            return Ok(ReviewerDTO);
        }

        //api/reviewers/{reviewerId}/reviews
        [HttpGet("{reviewerId}/reviews")]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            // check if Reviewer with Id exists
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            var reviews = _reviewerRepository.GetReviewsByReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewsDTO = new List<ReviewDTO>();
            foreach (var review in reviews)
            {
                reviewsDTO.Add(new ReviewDTO
                {
                    Id = review.Id,
                    Headline = review.Headline,
                    ReviewText = review.ReviewText,
                    Rating = review.Rating
                });
            }
            return Ok(reviewsDTO);
        }

        //api/reviewers/review/{reviewId}
        [HttpGet("review/{reviewId}")]
        public IActionResult GetReviewerOfAReview(int reviewId)
        {
            // check if review with Id exists
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var reviewer = _reviewerRepository.GetReviewerOfAReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ReviewerDTO = new ReviewerDTO()
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                Lastname = reviewer.LastName
            };
            return Ok(ReviewerDTO);
        }
    }
}
