using BookApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.Services
{
    public interface IBookRepository
    {
        ICollection<Book> GetBooks();
        Book GetBookById(int bookId);
        Book GetBookByIsbn(String Isbn);
        bool isExists(int bookId);
        bool isExists(String Isbn);
        bool IsDuplicateIsbn(String Isbn, int bookId);
        int GetBookRating(int bookId);
        bool CreateBook(Book book, List<int> authorsId, List<int> categoriesId);
        bool UpdateBook(Book book, List<int> authorsId, List<int> categoriesId);
        bool DeleteBook(Book book);
        bool SaveBook();
    }
}
