using BookApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.Services
{
    public class BookRepository : IBookRepository
    {
        private BookDbContext _bookContext;
        public BookRepository(BookDbContext bookContext)
        {
            _bookContext = bookContext;
        }

        public bool CreateBook(Book book, List<int> authorsId, List<int> categoriesId)
        {
            var authors = _bookContext.Authors.Where(a => authorsId.Contains(a.Id)).ToList();
            var categories = _bookContext.Categories.Where(c => categoriesId.Contains(c.Id)).ToList();

            foreach (var author in authors)
            {
                var bookAuthors = new BookAuthor
                {
                    Author = author,
                    Book = book
                };
                _bookContext.Add(bookAuthors);
            }

            foreach (var category in categories)
            {
                var bookCategories = new BookCategory
                {
                    Category = category,
                    Book = book
                };
                _bookContext.Add(bookCategories);
            }

            _bookContext.Add(book);
            return SaveBook();
        }

        public bool DeleteBook(Book book)
        {
            _bookContext.Remove(book);
            return SaveBook();
        }

        public Book GetBookById(int bookId)
        {
            return _bookContext.Books.Where(b => b.Id == bookId).FirstOrDefault();
        }

        public Book GetBookByIsbn(string Isbn)
        {
            return _bookContext.Books.Where(b => b.Isbn == Isbn).FirstOrDefault();
        }

        public int GetBookRating(int bookId)
        {
            return _bookContext.Reviews.Where(rv => rv.Book.Id == bookId).Select(b => b.Rating).FirstOrDefault();
        }

        public ICollection<Book> GetBooks()
        {
            return _bookContext.Books.ToList();
        }

        public bool IsDuplicateIsbn(string Isbn, int bookId)
        {
            var numberOfIsbn = _bookContext.Books.Where(b => b.Isbn == Isbn && b.Id == bookId).Count();
            return numberOfIsbn > 0 ? true : false;
        }

        public bool isExists(int bookId)
        {
            return _bookContext.Books.Where(b => b.Id == bookId).Any();
        }

        public bool isExists(string Isbn)
        {
            return _bookContext.Books.Where(b => b.Isbn == Isbn).Any();
        }

        public bool SaveBook()
        {
            var saved = _bookContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateBook(Book book, List<int> authorsId, List<int> categoriesId)
        {
            var authors = _bookContext.Authors.Where(a => authorsId.Contains(a.Id)).ToList();
            var categories = _bookContext.Categories.Where(c => categoriesId.Contains(c.Id)).ToList();

            var bookAuthorsToDelete = _bookContext.BookAuthors.Where(b => b.BookId == book.Id);
            var bookCategoriesToDelete = _bookContext.BookCategories.Where(b => b.BookId == book.Id);

            _bookContext.RemoveRange(bookAuthorsToDelete);
            _bookContext.RemoveRange(bookCategoriesToDelete);

            foreach (var author in authors)
            {
                var bookAuthors = new BookAuthor
                {
                    Author = author,
                    Book = book
                };
                _bookContext.Add(bookAuthors);
            }

            foreach (var category in categories)
            {
                var bookCategories = new BookCategory
                {
                    Category = category,
                    Book = book
                };
                _bookContext.Add(bookCategories);
            }

            _bookContext.Update(book);
            return SaveBook();
        }
    }
}
