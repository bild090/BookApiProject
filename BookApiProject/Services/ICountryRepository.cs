using BookApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.Services
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int countryId);
        ICollection<Author> GetAuthoresFromCountry(int countryId);
        bool IsDuplicateCountry(String name, int countryId);
        Country GetCountryOfAuthor(int authorId);
        bool isExists(int countryId);
        bool CreateCaountry(Country country);
        bool UpdateCaountry(Country country);
        bool DeleteCaountry(Country country);
        bool SaveCaountry();
    }
}
