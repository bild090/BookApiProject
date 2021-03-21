using BookApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.Services
{
    public interface ICatrgoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);
        ICollection<Category> GetCategoriesForBook(int bookId);
        ICollection<Book> GetBooksPerCategory(int categoryId);
        bool IsDuplicateCategory(String name, int categoryId);

        bool isExists(int categoryId);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool SaveCategory();
    }
}
