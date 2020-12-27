using BookProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookProject.Services
{
    public class CountryRepository : ICountryRepository
    {
        private BookDbContext _bookDbContext;

        public CountryRepository(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }
        public ICollection<Author> GetAuthorsFromACountry(int countryId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Country> GetCountries()
        {
            return _bookDbContext.Countries.OrderBy(c => c.Name).ToList();
        }

        public Country GetCountry(int countryId)
        {
            return _bookDbContext.Countries.Where(c => c.Id == countryId).FirstOrDefault();
        }

        public Country GetCountryOfAnAuthor(int authorId)
        {
            throw new NotImplementedException();
        }
    }
}
