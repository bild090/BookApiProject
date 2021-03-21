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
    public class BooksController : Controller
    {
        private IBookRepository _bookRepo;
        private IAuthorRepository _authorRepo;
        private ICatrgoryRepository _categoryRepo;
        private IReviewRepository _reviewRepo;

        public BooksController(IBookRepository bookRepo, IAuthorRepository authorRepo, ICatrgoryRepository categoryRepo, IReviewRepository reviewRepo)
        {
            _bookRepo = bookRepo;
            _authorRepo = authorRepo;
            _categoryRepo = categoryRepo;
            _reviewRepo = reviewRepo;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        public IActionResult GetBooks()
        {

            if (!ModelState.IsValid)
                return BadRequest(400);

            var books = _bookRepo.GetBooks();

            var booksDto = new List<BookDto>();

            foreach (var book in books)
            {
                booksDto.Add(new BookDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Isbn = book.Isbn,
                    DatePublished = book.DatePublished
                });
            }
            return Ok(booksDto);
        }

        [HttpGet("{bookId}", Name = "GetBookById")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        public IActionResult GetBookById(int bookId)
        {
            if (!_bookRepo.isExists(bookId))
                return NotFound();

            var book = _bookRepo.GetBookById(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Isbn = book.Isbn,
                DatePublished = book.DatePublished

            };
            return Ok(bookDto);
        }

        [HttpGet("{bookId}/rating")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        public IActionResult GetBookRating(int bookId)
        {
            if (!_bookRepo.isExists(bookId))
                return NotFound();

            var rating = _bookRepo.GetBookRating(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookRating = new BookRatingDto
            {
                Rating = rating
            };
            return Ok(bookRating);
        }

        [HttpPost]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(201, Type = typeof(Book))]
        public IActionResult CreateBook([FromBody] Book bookToCreate, [FromQuery] List<int> authorsId, [FromQuery] List<int> categoriesId)
        {
            var statusCode = ValidateBook(bookToCreate, authorsId, categoriesId);

            if (_bookRepo.IsDuplicateIsbn(bookToCreate.Isbn, bookToCreate.Id))
            {
                ModelState.AddModelError("", " Duplicate ISBN");
                return StatusCode(422);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", $"Something went wrong while createing {bookToCreate.Title} ");
                return StatusCode(statusCode.StatusCode);
            }

            if (!_bookRepo.CreateBook(bookToCreate, authorsId, categoriesId))
            {
                ModelState.AddModelError("", $"Something went wrong while createing {bookToCreate.Title} ");
                return StatusCode(statusCode.StatusCode);
            }
            return CreatedAtRoute("GetBookById", new { bookId = bookToCreate.Id }, bookToCreate);
        }

        [HttpPut("{bookId}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]
        [ProducesResponseType(422)]
        [ProducesResponseType(201, Type = typeof(Book))]
        public IActionResult UpdateBook(int bookId, [FromBody] Book bookToUpdate, [FromQuery] List<int> authorsId, [FromQuery] List<int> categoriesId)
        {
            var statusCode = ValidateBook(bookToUpdate, authorsId, categoriesId);

            if (bookId != bookToUpdate.Id)
                return BadRequest();

            if (_authorRepo.isExists(bookId))
                return NotFound();

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", $"Something went wrong while createing {bookToUpdate.Title} ");
                return StatusCode(statusCode.StatusCode);
            }

            if (!_bookRepo.UpdateBook(bookToUpdate, authorsId, categoriesId))
            {
                ModelState.AddModelError("", $"Something went wrong while updating {bookToUpdate.Title} ");
                return StatusCode(statusCode.StatusCode);
            }
            return NoContent();
        }

        [HttpDelete("{bookId}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public IActionResult DeleteBook(int bookId)
        {
            if(!_bookRepo.isExists(bookId))
            {
                ModelState.AddModelError("", "Book does not exists");
                return BadRequest();
            }

            var reviews = _reviewRepo.GetReviewsOfBook(bookId);
            var book = _bookRepo.GetBookById(bookId);

            if (!ModelState.IsValid)
                BadRequest(ModelState);

            if(!_reviewRepo.DeleteReviews(reviews.ToList()))
            {
                ModelState.AddModelError("", "somthing went wrong with delete reviews");
                return StatusCode(500, ModelState);
            }

            if(!_bookRepo.DeleteBook(book))
            {
                ModelState.AddModelError("", "somthing went wrong with delete Book");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        private StatusCodeResult ValidateBook(Book book, List<int> authorsId, List<int> categoriesId)
        {
            if (book == null || authorsId.Count() <= 0 || categoriesId.Count() <= 0)
            {
                ModelState.AddModelError("", "Missing Book, Auhours or Categories");
                return BadRequest();
            }

            foreach (var id in authorsId)
            {
                if(!_authorRepo.isExists(id))
                {
                    ModelState.AddModelError("", "Author Not Found");
                    return BadRequest();
                }
            }

            foreach (var id in categoriesId)
            {
                if (!_authorRepo.isExists(id))
                {
                    ModelState.AddModelError("", "Category Not Found");
                    return StatusCode(404);
                }
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Criteicale Error");
                return BadRequest();
            }

            return NoContent();
        }
    }
}
