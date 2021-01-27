using BookProject.DTOs;
using BookProject.Models;
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
        [HttpGet("{reviewerId}", Name = "GetReviewer")]
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

        //api/reviewers
        [HttpPost]
        public IActionResult CreateReviewer([FromBody] Reviewer reviewerNeedToCreate)
        {
            if (reviewerNeedToCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepository.CreateReviewer(reviewerNeedToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong creating {reviewerNeedToCreate.FirstName}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetReviewer", new { reviewerId = reviewerNeedToCreate.Id }, reviewerNeedToCreate);
        }

        //api/countries/reviewerId
        [HttpPut("{reviewerId}")]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] Reviewer reviewerNeedToUpdate)
        {
            if (reviewerNeedToUpdate == null)
                return BadRequest(ModelState);

            if (reviewerNeedToUpdate.Id != reviewerId)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepository.UpdateReviewer(reviewerNeedToUpdate))
            {
                ModelState.AddModelError("", $"Something went wrong updating {reviewerNeedToUpdate.FirstName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //api/countries/reviewerId
        [HttpDelete("{reviewerId}")]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            var ReviewerToDelete = _reviewerRepository.GetReviewer(reviewerId);
            var ReviewsToDelete = _reviewerRepository.GetReviewsByReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepository.DeleteReviewer(ReviewerToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting {ReviewerToDelete.FirstName}");
                return StatusCode(500, ModelState);
            }

            // Delete all reviews of a reviewers
            if (!_reviewRepository.DeleteReviews(ReviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", $"Something went wrong deleting reviews by {ReviewerToDelete.FirstName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
