using BookApiProject.DTOs;
using BookApiProject.Models;
using BookApiProject.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : Controller
    {
        private IReviewRepository _reviewRepo;

        public ReviewsController(IReviewRepository reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }

        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(List<ReviewDto>))]
        public IActionResult GetReviews()
        {
            var reviews = _reviewRepo.GetReviews();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var revewsDto = new List<ReviewDto>();

            foreach (var review in reviews)
            {
                revewsDto.Add(new ReviewDto
                {
                    Id = review.Id,
                    Headline = review.Headline,
                    ReviewText = review.ReviewText,
                    Rating = review.Rating
                });   
            }
            return Ok(revewsDto);
        }

        [HttpGet("{reviewId}", Name = "GetReview")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepo.isExists(reviewId))
                return NotFound();

            var review = _reviewRepo.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerDto = new ReviewDto
            {
                Id = review.Id,
                Headline =review.Headline,
                ReviewText = review.ReviewText,
                Rating =review.Rating
            };
            return Ok(reviewerDto);
        }

        [HttpGet("{bookId}/reviews")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        public IActionResult GetReviewsOfBook(int bookId)
        {
            if (!_reviewRepo.isExists(bookId))
                return NotFound();

            var reviews = _reviewRepo.GetReviewsOfBook(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewDto = new List<ReviewDto>();

            foreach (var review in reviews)
            {
                reviewDto.Add(new ReviewDto
                {
                    Id = review.Id,
                    Headline = review.Headline,
                    ReviewText = review.ReviewText,
                    Rating = review.Rating
                });
            }

            return Ok(reviewDto);
        }

        [HttpGet("{reviewId}/book")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        public IActionResult GetBookOfReviw(int reviewId)
        {
            if (!_reviewRepo.isExists(reviewId))
                return NotFound();

            var book = _reviewRepo.GetBookOfReviw(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookDto = new BookDto
            {
                Id = book.Id,
                Isbn = book.Isbn,
                Title =book.Title,
                DatePublished = book.DatePublished
                
            };
            return Ok(bookDto);
        }
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateReview(Review createReview)
        {
            if (createReview == null)
                return BadRequest(ModelState);

            if (_reviewRepo.isExists(createReview.Id))
            {
                ModelState.AddModelError("", $"Category {createReview.ReviewText} is aleardy exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_reviewRepo.CreateReview(createReview))
            {
                ModelState.AddModelError("", $"Somthing went wrong during creating {createReview.ReviewText}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetReview", new { reviewId = createReview.Id }, createReview);
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(204)]
        public IActionResult UpdateCountry(int reviewId, [FromBody] Review reviewToUpdate)
        {
            if (reviewToUpdate == null)
                return BadRequest(ModelState);

            if (reviewId != reviewToUpdate.Id)
                return BadRequest(ModelState);

            if (!_reviewRepo.isExists(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepo.UpdateReview(reviewToUpdate))
            {
                ModelState.AddModelError("", $"somthing went wrong during update {reviewToUpdate.ReviewText}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(204)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepo.isExists(reviewId))
                return NotFound();

            var review = _reviewRepo.GetReview(reviewId);

            if (_reviewRepo.GetReviewsOfBook(reviewId).Count() > 0)
            {
                ModelState.AddModelError("", $"Sorry we can't delete Review {review.ReviewText} while at least has one Author");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepo.DeleteReview(review))
            {
                ModelState.AddModelError("", $"Sorry we can't delete Review {review.ReviewText} somthing went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
