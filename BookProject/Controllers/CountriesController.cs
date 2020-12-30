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
        [HttpGet("{countryId}")]
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
    }
}
