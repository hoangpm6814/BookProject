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
    public class CountriesController : Controller
    {
        private ICountryRepository _countryRepository;
        private IAuthorRepository _authorRepository;

        public CountriesController(ICountryRepository countryRepository, IAuthorRepository authorRepository)
        {
            _countryRepository = countryRepository;
            _authorRepository = authorRepository;
        }

        //api/countries
        [HttpGet]
        public IActionResult GetCountries()
        {
            var countries = _countryRepository.GetCountries().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countriesDTO = new List<CountryDTO>();
            foreach(var country in countries)
            {
                countriesDTO.Add(new CountryDTO
                {
                    Id = country.Id,
                    Name = country.Name
                });
            }
            return Ok(countriesDTO);
        }

        //api/countries/countryId
        [HttpGet("{countryId}", Name = "GetCountry")]
        public IActionResult GetCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            var country = _countryRepository.GetCountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryDTO = new CountryDTO()
            {
                Id = country.Id,
                Name = country.Name
            };
            return Ok(countryDTO);
        }

        //api/countries/authors/authorId
        [HttpGet("authors/{authorId}")]
        public IActionResult GetCountryOfAnAuthor(int authorId)
        {
            // Check author with Id exists
            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();

            var country = _countryRepository.GetCountryOfAnAuthor(authorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryDTO = new CountryDTO()
            {
                Id = country.Id,
                Name = country.Name
            };
            return Ok(countryDTO);
        }

        //api/countries/countryId/authors
        [HttpGet("{countryId}/authors")] 
        public IActionResult GetAuthorsFromACountry(int countryId)
        {
            // Check country with Id exists
            if (!_countryRepository.CountryExists(countryId))
                return NotFound(); 

            var authors = _countryRepository.GetAuthorsFromACountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorsDTO = new List<AuthorDTO>();

            foreach(var author in authors)
            {
                authorsDTO.Add(new AuthorDTO()
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                });
            }

            return Ok(authorsDTO);
        }

        //api/countries
        [HttpPost]
        public IActionResult CreateCountry([FromBody]Country countryNeedToCreate)
        {
            if (countryNeedToCreate == null)
                return BadRequest(ModelState);

            // Check whether country's name exists in DB or not. 
            // Should be done in Repo, not here.
            var country = _countryRepository.GetCountries()
                .Where(c => c.Name.Trim().ToUpper() == countryNeedToCreate.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", $"Country {countryNeedToCreate.Name} already exists.");
                return StatusCode(442, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepository.CreateCountry(countryNeedToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong creating {countryNeedToCreate.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCountry", new { countryId = countryNeedToCreate.Id }, countryNeedToCreate);
        }

        //api/countries/countryId
        [HttpPut("{countryId}")]
        public IActionResult UpdateCountry(int countryId, [FromBody] Country countryNeedToUpdate)
        {
            if (countryNeedToUpdate == null)
                return BadRequest(ModelState);

            if (countryNeedToUpdate.Id != countryId)
                return BadRequest(ModelState);

            if (_countryRepository.IsDuplicateCountryName(countryId, countryNeedToUpdate.Name))
            {
                ModelState.AddModelError("", $"Country {countryNeedToUpdate.Name} already exists.");
                return StatusCode(442, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepository.UpdateCountry(countryNeedToUpdate))
            {
                ModelState.AddModelError("", $"Something went wrong updating {countryNeedToUpdate.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //api/countries/countryId
        [HttpDelete("{countryId}")]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            var countryToDelete = _countryRepository.GetCountry(countryId);

            if (_countryRepository.GetAuthorsFromACountry(countryId).Count() > 0)
            {
                ModelState.AddModelError("", $"{countryToDelete.Name} cannot be deleted because it is used by at least one author.");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepository.DeleteCountry(countryToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting {countryToDelete.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
