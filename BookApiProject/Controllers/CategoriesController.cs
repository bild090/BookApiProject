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
    public class CategoriesController : Controller
    {
        private ICatrgoryRepository _categoryRepo;

        public CategoriesController(ICatrgoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoriesDto>))]
        public IActionResult GetCategories() {

            if (!ModelState.IsValid)
                return BadRequest(400);

            var categories = _categoryRepo.GetCategories();

            var categoriesDto = new List<CategoriesDto>();

            foreach (var category in categories)
            {
                categoriesDto.Add(new CategoriesDto
                {
                    Id = category.Id,
                    Name = category.Name

                });
            }

            return Ok(categoriesDto);
        }

        [HttpGet("{categoryId}", Name = "GetCategory")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoriesDto>))]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_categoryRepo.isExists(categoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(400);

            var category = _categoryRepo.GetCategory(categoryId);

            var categoriesDto = new CategoriesDto()
            {
                Id = category.Id,
                Name = category.Name

            };
            return Ok(categoriesDto);
        }

        [HttpGet("book/{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        public IActionResult GetBooksPerCategory(int categoryId) 
        {
            if (!ModelState.IsValid)
                return NotFound();

            if (!_categoryRepo.isExists(categoryId))
                return BadRequest();

            var books = _categoryRepo.GetBooksPerCategory(categoryId).ToList();

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

        [HttpGet("books/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoriesDto>))]
        public IActionResult GetCategoriesForBook(int bookId)
        {
            if (!ModelState.IsValid)
                return NotFound();

            if (!_categoryRepo.isExists(bookId))
                return BadRequest();

            var categories = _categoryRepo.GetCategoriesForBook(bookId).ToList();

            var categoryDto = new List<CategoriesDto>();

            foreach (var category in categories)

                categoryDto.Add(new CategoriesDto
                {
                    Id = category.Id,
                    Name = category.Name
                });


            return Ok(categoryDto);
        }

        [HttpPost]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(201, Type = typeof(Category))]
        public IActionResult CreateCategore([FromBody] Category categoryToCareate)
        {
            if (categoryToCareate == null)
                return BadRequest(ModelState);

            var country = _categoryRepo.GetCategories().Where(c => c.Name.Trim().ToUpper() == categoryToCareate.Name.Trim().ToUpper()).Count();

            if (country > 0)
            {
                ModelState.AddModelError("", $"Category {categoryToCareate.Name} is aleardy exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepo.CreateCategory(categoryToCareate))
            {
                ModelState.AddModelError("", $"Somthing went wrong during save {categoryToCareate.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCategory", new { categoryId = categoryToCareate.Id }, categoryToCareate);
        }
        [HttpPut("{categoryId}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(204)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] Category categoryToUpdate)
        {
            if (categoryToUpdate == null)
                return BadRequest(ModelState);

            if (categoryId != categoryToUpdate.Id)
                return BadRequest(ModelState);

            if (_categoryRepo.IsDuplicateCategory(categoryToUpdate.Name, categoryId))
            {
                ModelState.AddModelError("", $"Category {categoryToUpdate.Name} is aleardy exists");
                return StatusCode(422, ModelState);
            }
            if (!_categoryRepo.isExists(categoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepo.UpdateCategory(categoryToUpdate))
            {
                ModelState.AddModelError("", $"somthing went wrong during update {categoryToUpdate.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(204)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRepo.isExists(categoryId))
                return NotFound();

            var category = _categoryRepo.GetCategory(categoryId);

            if (_categoryRepo.GetBooksPerCategory(categoryId).Count() > 0)
            {
                ModelState.AddModelError("", $"Sorry we can't delete country{category.Name} while at least has one Author");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepo.DeleteCategory(category))
            {
                ModelState.AddModelError("", $"Sorry we can't delete country{category.Name} somthing went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
