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
    public class AuthorsController : Controller
    {
        private IAuthorRepository _authorRepo;

        public AuthorsController(IAuthorRepository authorRepo)
        {
            _authorRepo = authorRepo;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDto>))]
        public IActionResult GetAuthors()
        {
            var authors = _authorRepo.GetAuthors();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorsDto = new List<AuthorDto>();

            foreach (var author in authors)
            {
                authorsDto.Add(new AuthorDto
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                });
            }
            return Ok(authorsDto);
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(AuthorDto))]
        public IActionResult GetAuthor(int authorId)
        {
            if (!_authorRepo.isExists(authorId))
                return NotFound();

            var author = _authorRepo.GetAuthor(authorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorDto = new AuthorDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName
            };
            return Ok(authorDto);
        }

        [HttpGet("{authorId}/books")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        public IActionResult GetBooksByAuthor(int authorId)
        {
            if (!_authorRepo.isExists(authorId))
                return NotFound();

            var books = _authorRepo.GetBooksByAuthor(authorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var booksDto = new List<BookDto>();

            foreach (var book in books)
            {
                booksDto.Add(new BookDto
                {
                    Id = book.Id,
                    Isbn = book.Isbn,
                    Title = book.Title,
                    DatePublished = book.DatePublished
                });
            }
            return Ok(booksDto);
        }

        [HttpGet("{bookId}/authors")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(AuthorDto))]
        public IActionResult GetAuthorsOfBook(int bookId)
        {
            if (!_authorRepo.isExists(bookId))
                return NotFound();

            var authors = _authorRepo.GetAuthorsOfBook(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorsDto = new List<AuthorDto>();

            foreach (var author in authors)
            {
                authorsDto.Add(new AuthorDto
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                });
            }
            return Ok(authorsDto);
        }

        [HttpPost]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(201, Type = typeof(Author))]
        public IActionResult CreateAuthor([FromBody] Author authorToCareate)
        {
            if (authorToCareate == null)
                return BadRequest(ModelState);

            if (_authorRepo.isExists(authorToCareate.Id))
            {
                ModelState.AddModelError("", $"Author {authorToCareate.FirstName} is aleardy exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_authorRepo.CreateAuthor(authorToCareate))
            {
                ModelState.AddModelError("", $"Somthing went wrong during save {authorToCareate.FirstName}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetAuthor", new { authorId = authorToCareate.Id }, authorToCareate);
        }
        [HttpPut("{authorId}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(204)]
        public IActionResult UpdateAuthor(int authorId, [FromBody] Author authorToUpdate)
        {
            if (authorToUpdate == null)
                return BadRequest(ModelState);

            if (authorId != authorToUpdate.Id)
                return BadRequest(ModelState);

            if (!_authorRepo.isExists(authorId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_authorRepo.UpdateAuthor(authorToUpdate))
            {
                ModelState.AddModelError("", $"somthing went wrong during update {authorToUpdate.FirstName}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{authorId}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(204)]
        public IActionResult DeleteAuthor(int authorId)
        {
            if (!_authorRepo.isExists(authorId))
                return NotFound();

            var author = _authorRepo.GetAuthor(authorId);

            if (_authorRepo.GetBooksByAuthor(authorId).Count() > 0)
            {
                ModelState.AddModelError("", $"Sorry we can't delete author{author.FirstName} while at least has one Author");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_authorRepo.DeleteAuthor(author))
            {
                ModelState.AddModelError("", $"Sorry we can't delete author{author.FirstName} somthing went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
