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
    public class ReviewsController : Controller
    {
        private IReviewRepository _reviewRepository;
        private IBookRepository _bookRepository;

        public ReviewsController(IReviewRepository reviewRepository, IBookRepository bookRepository)
        {
            _reviewRepository = reviewRepository;
            _bookRepository = bookRepository;
        }

        //api/reviews
        [HttpGet]
        public IActionResult GetReviews()
        {
            var reviews = _reviewRepository.GetReviews().ToList();

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

        //api/reviews/reviewId
        [HttpGet("{reviewId}", Name = "GetReview")]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var review = _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ReviewDTO = new ReviewDTO()
            {
                Id = review.Id,
                Headline = review.Headline,
                ReviewText = review.ReviewText,
                Rating = review.Rating
            };
            return Ok(ReviewDTO);
        }

        //api/reviews/book/bookId
        [HttpGet("book/{bookId}")]
        public IActionResult GetReviewsOfABook(int bookId)
        {
            // Check book with Id exists
            if (!_bookRepository.BookExists(bookId))
                return NotFound();

            var reviews = _reviewRepository.GetReviewsOfABook(bookId);

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

        //api/reviews/{reviewId}/book
        [HttpGet("{reviewId}/book")]
        public IActionResult GetBookOfAReview(int reviewId)
        {
            // Check review with Id exists
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var book = _reviewRepository.GetBookOfAReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookDTO = new BookDTO()
            {
                Id = book.Id,
                Title = book.Title,
                Isbn = book.Isbn,
                DatePublished = book.DatePublished
            };
            return Ok(bookDTO);
        }

        //api/reviews
        [HttpPost]
        public IActionResult CreateReview([FromBody] Review ReviewNeedToCreate)
        {
            if (ReviewNeedToCreate == null)
                return BadRequest(ModelState);

            //if (!_reviewRepository.ReviewExists(ReviewNeedToCreate.Reviewer.Id))
            //    ModelState.AddModelError("", $"Reviewer doesn't exists.");

            //if (!_bookRepository.BookExists(ReviewNeedToCreate.Book.Id))
            //    ModelState.AddModelError("", $"Book doesn't exists.");

            //if (!ModelState.IsValid)
            //    return StatusCode(404, ModelState);

            //ReviewNeedToCreate.Book = _bookRepository.GetBook(ReviewNeedToCreate.Book.Id);
            //ReviewNeedToCreate.Reviewer = _reviewRepository.GetReviewer(ReviewNeedToCreate.Reviewer.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.CreateReview(ReviewNeedToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong creating {ReviewNeedToCreate.Headline}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetReview", new { reviewId = ReviewNeedToCreate.Id }, ReviewNeedToCreate);
        }

        //api/countries/reviewId
        [HttpPut("{reviewId}")]
        public IActionResult UpdateReview(int reviewId, [FromBody] Review ReviewNeedToUpdate)
        {
            if (ReviewNeedToUpdate == null)
                return BadRequest(ModelState);

            if (ReviewNeedToUpdate.Id != reviewId)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.UpdateReview(ReviewNeedToUpdate))
            {
                ModelState.AddModelError("", $"Something went wrong updating {ReviewNeedToUpdate.Headline}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //api/countries/reviewId
        [HttpDelete("{reviewId}")]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var ReviewToDelete = _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReview(ReviewToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting {ReviewToDelete.Headline}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
