using BookApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.Services
{
    public class CategoryRepository : ICatrgoryRepository
    {
        private BookDbContext _categoryContext;

        public CategoryRepository(BookDbContext categoryContext)
        {
            _categoryContext = categoryContext;
        }

        public bool CreateCategory(Category category)
        {
            _categoryContext.Add(category);
            return SaveCategory();
        }

        public bool DeleteCategory(Category category)
        {
            _categoryContext.Remove(category);
            return SaveCategory();
        }

        public ICollection<Book> GetBooksPerCategory(int categoryId)
        {
            return _categoryContext.BookCategories.Where(c => c.CategoryId == categoryId).Select(b => b.Book).ToList();
            
        }

        public ICollection<Category> GetCategories()
        {
            return _categoryContext.Categories.ToList();
        }

        public ICollection<Category> GetCategoriesForBook(int bookId)
        {
            return _categoryContext.BookCategories.Where(b => b.BookId == bookId).Select(c => c.Category).ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _categoryContext.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
        }

        public bool IsDuplicateCategory(string name, int categoryId)
        {
            var numberOfIsbn = _categoryContext.Categories.Where(c => c.Name.Trim().ToUpper() == name.Trim().ToUpper() && c.Id == categoryId).Count();
            return numberOfIsbn > 0 ? true : false;
        }

        public bool isExists(int categoryId)
        {
            return _categoryContext.Categories.Where(c => c.Id == categoryId).Any();
        }

        public bool SaveCategory()
        {
            var saved = _categoryContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _categoryContext.Update(category);
            return SaveCategory();
        }
    }
}
