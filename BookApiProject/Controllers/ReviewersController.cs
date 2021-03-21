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
    public class ReviewersController : Controller
    {
        private IReviewerRepository _reviewerRepo;
        private IReviewRepository _reviewRepo;
        public ReviewersController(IReviewerRepository reviewerRepo, IReviewRepository reviewRepo)
        {
            _reviewerRepo = reviewerRepo;
            _reviewRepo = reviewRepo;
        }
        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(List<ReviewerDto>))]
        public IActionResult GetReviewers()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewers = _reviewerRepo.GetReviewers();

            var reviewersDto = new List<ReviewerDto>();

            foreach (var reviewer in reviewers)
            {
                reviewersDto.Add(new ReviewerDto
                {
                    Id = reviewer.Id,
                    FirstName = reviewer.FirstName,
                    LastName = reviewer.LastName
                });
            }

            return Ok(reviewersDto);
        }

        [HttpGet("{reviewerId}", Name = "GetReviewer")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(ReviewerDto))]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_reviewerRepo.isExists(reviewerId))
                return NotFound();

            var reviewer = _reviewerRepo.GetReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerDto = new ReviewerDto
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                LastName = reviewer.LastName
            };
            return Ok(reviewerDto);

        }
        
        [HttpGet("{reviewId}/reviewer")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(ReviewerDto))]
        public IActionResult GetReviewerOfReview(int reviewId)
        {
            if (!_reviewerRepo.isExists(reviewId))
                return NotFound();

            var reviewer = _reviewerRepo.GetReviewerOfReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerDto = new ReviewerDto
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                LastName = reviewer.LastName
            };
            return Ok(reviewerDto);
        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(List<ReviewDto>))]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            if (!_reviewerRepo.isExists(reviewerId))
                return NotFound();

            var reviews = _reviewerRepo.GetReviewsByReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewsDto = new List<ReviewDto>();

            foreach (var review in reviews)
            {
                reviewsDto.Add(new ReviewDto
                { 
                    Id = review.Id,
                    Headline = review.Headline,
                    ReviewText = review.ReviewText,
                    Rating = review.Rating
                });
            }

            return Ok(reviewsDto);
        }

        [HttpPost]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(201, Type = typeof(Reviewer))]
        public IActionResult CreateReviewer([FromBody] Reviewer reviewerToCareate)
        {
            if (reviewerToCareate == null)
                return BadRequest(ModelState);

            if (_reviewerRepo.isExists(reviewerToCareate.Id))
            {
                ModelState.AddModelError("", $"Reviewer {reviewerToCareate.FirstName} is aleardy exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepo.CreateReviewer(reviewerToCareate))
            {
                ModelState.AddModelError("", $"Somthing went wrong during save {reviewerToCareate.FirstName}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetReviewer", new { reviewerId = reviewerToCareate.Id }, reviewerToCareate);
        }
        [HttpPut("{reviewerId}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(204)]
        public IActionResult UpdateCountry(int reviewerId, [FromBody] Reviewer reviewerToUpdate)
        {
            if (reviewerToUpdate == null)
                return BadRequest(ModelState);

            if (reviewerId != reviewerToUpdate.Id)
                return BadRequest(ModelState);

            if (!_reviewerRepo.isExists(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepo.UpdateReviewer(reviewerToUpdate))
            {
                ModelState.AddModelError("", $"somthing went wrong during update {reviewerToUpdate.FirstName}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(204)]
        public IActionResult DeleteCountry(int reviewerId)
        {
            if (!_reviewerRepo.isExists(reviewerId))
                return NotFound();

            var reviewer = _reviewerRepo.GetReviewer(reviewerId);

            if (_reviewerRepo.GetReviewsByReviewer(reviewerId).Count() > 0)
            {
                ModelState.AddModelError("", $"Sorry we can't delete reviewer{reviewer.FirstName} while at least has one Author");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepo.DeleteReviewer(reviewer))
            {
                ModelState.AddModelError("", $"Sorry we can't delete reviewer{reviewer.FirstName} somthing went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
