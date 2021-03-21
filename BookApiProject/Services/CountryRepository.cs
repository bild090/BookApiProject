using BookApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.Services
{
    public class CountryRepository : ICountryRepository
    {
        private BookDbContext _countryContext;
        public CountryRepository(BookDbContext countryContext)
        {
            _countryContext = countryContext;
        }

        public bool CreateCaountry(Country country)
        {
            _countryContext.Add(country);
            return SaveCaountry();
        }

        public bool DeleteCaountry(Country country)
        {
            _countryContext.Remove(country);
            return SaveCaountry();
        }

        public ICollection<Author> GetAuthoresFromCountry(int countryId)
        {
            return _countryContext.Authors.Where(c => c.Country.Id == countryId).ToList();
        }

        public ICollection<Country> GetCountries()
        {
            return _countryContext.Countries.OrderBy(c => c.Name).ToList();
        }

        public Country GetCountry(int countryId)
        {
            return _countryContext.Countries.Where(c => c.Id == countryId).FirstOrDefault();
        }

        public Country GetCountryOfAuthor(int authorId)
        {
            return _countryContext.Authors.Where(a => a.Id == authorId).Select(c => c.Country).FirstOrDefault();
        }

        public bool IsDuplicateCountry(string name, int countryId)
        {
            var numberOfIsbn = _countryContext.Countries.Where(c => c.Name.Trim().ToUpper() == name.Trim().ToUpper() && c.Id == countryId).Count();
            return numberOfIsbn > 0 ? true : false;
        }

        public bool isExists(int countryId)
        {
            return _countryContext.Countries.Any(c => c.Id == countryId);
        }

        public bool SaveCaountry()
        {
            var saved = _countryContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateCaountry(Country country)
        {
            _countryContext.Update(country);
            return SaveCaountry();
        }
    }
}
