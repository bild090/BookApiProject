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
    public class CountriesController : Controller
    {
        private ICountryRepository _countryRepo;
        public CountriesController(ICountryRepository countryRepo)
        {
            _countryRepo = countryRepo;
        }

        //api/countries
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CountryDto>))]
        public IActionResult GetCountries()
        {
            var countries = _countryRepo.GetCountries().ToList();

            if (!ModelState.IsValid)
                return BadRequest();

            var countryDto = new List<CountryDto>();

            foreach (var country in countries)
            {
                countryDto.Add(new CountryDto
                {
                    Id = country.Id,
                    Name = country.Name
                });
            }
            return Ok(countryDto);
        }

        //api/countries/countryId
        [HttpGet("{countryId}", Name = "GetCountry")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        public IActionResult GetCountry(int countryId) 
        {
            if (!_countryRepo.isExists(countryId))
                return NotFound();

            var country = _countryRepo.GetCountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryDto = new CountryDto()
            {
                Id = country.Id,
                Name = country.Name
            };

            return Ok(countryDto);
        }

        //api/countries/authors/authorId
        [HttpGet("authors/{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        public IActionResult GetCountryOfAuthor(int authorId)
        {
            if (!_countryRepo.isExists(authorId))
                return NotFound();

            var country = _countryRepo.GetCountryOfAuthor(authorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryDto = new CountryDto()
            {
                Id = country.Id,
                Name = country.Name
            };

            return Ok(countryDto);
        }

        [HttpGet("{countryId}/authors")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDto>))]
        public IActionResult GetAuthoresFromCountry(int countryId)
        {
            if (!ModelState.IsValid)
                return NotFound();

            if (!_countryRepo.isExists(countryId))
                return BadRequest();

            var authors = _countryRepo.GetAuthoresFromCountry(countryId);

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
        [ProducesResponseType(201, Type = typeof(Country))]
        public IActionResult CreateCountry([FromBody] Country countryToCareate)
        {
            if (countryToCareate == null)
                return BadRequest(ModelState);

            var country = _countryRepo.GetCountries().Where(c => c.Name.Trim().ToUpper() == countryToCareate.Name.Trim().ToUpper()).Count();

            if(country > 0)
            {
                ModelState.AddModelError("", $"Country {countryToCareate.Name} is aleardy exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepo.CreateCaountry(countryToCareate))
            {
                ModelState.AddModelError("", $"Somthing went wrong during save {countryToCareate.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCountry", new { countryId = countryToCareate.Id}, countryToCareate);
        }
        [HttpPut("{countryId}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(204)]
        public IActionResult UpdateCountry(int countryId, [FromBody]Country countryToUpdate)
        {
            if (countryToUpdate == null)
                return BadRequest(ModelState);

            if (countryId != countryToUpdate.Id)
                return BadRequest(ModelState);

            if (_countryRepo.IsDuplicateCountry(countryToUpdate.Name, countryId))
            { 
            ModelState.AddModelError("", $"Country {countryToUpdate.Name} is aleardy exists");
            return StatusCode(422, ModelState);
            }
            if (!_countryRepo.isExists(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_countryRepo.UpdateCaountry(countryToUpdate))
            {
                ModelState.AddModelError("", $"somthing went wrong during update {countryToUpdate.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(204)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryRepo.isExists(countryId))
                return NotFound();

            var country = _countryRepo.GetCountry(countryId);

            if(_countryRepo.GetAuthoresFromCountry(countryId).Count() > 0)
            {
                ModelState.AddModelError("", $"Sorry we can't delete country{country.Name} while at least has one Author");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(! _countryRepo.DeleteCaountry(country))
            {
                ModelState.AddModelError("", $"Sorry we can't delete country{country.Name} somthing went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
