using BookApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        private BookDbContext _authorContext;

        public AuthorRepository(BookDbContext authorContext)
        {
            _authorContext = authorContext;
        }

        public bool CreateAuthor(Author author)
        {
            _authorContext.Add(author);
            return SaveAuthor();
        }

        public bool DeleteAuthor(Author author)
        {
            _authorContext.Remove(author);
            return SaveAuthor();
        }

        public Author GetAuthor(int authorId)
        {
            return _authorContext.Authors.Where(a => a.Id == authorId).FirstOrDefault();
        }

        public ICollection<Author> GetAuthors()
        {
            return _authorContext.Authors.OrderBy(a => a.Id).ToList();
        }

        public ICollection<Author> GetAuthorsOfBook(int bookId)
        {
            return _authorContext.BookAuthors.Where(b => b.BookId == bookId).Select(a => a.Author).ToList();
        }

        public ICollection<Book> GetBooksByAuthor(int authorId)
        {
            return _authorContext.BookAuthors.Where(a => a.Author.Id == authorId).Select(b => b.Book).ToList();
        }

        public bool isExists(int authorId)
        {
            return _authorContext.Authors.Where(a => a.Id == authorId).Any();
        }

        public bool SaveAuthor()
        {
            var saved = _authorContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateAuthor(Author author)
        {
            _authorContext.Update(author);
            return SaveAuthor();
        }
    }
}
