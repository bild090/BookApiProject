using BookApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.Services
{
    public class ReviewRepository : IReviewRepository
    {
        private BookDbContext _reviewContext;
        public ReviewRepository(BookDbContext reviewContext)
        {
            _reviewContext = reviewContext;
        }

        public bool CreateReview(Review review)
        {
            _reviewContext.Add(review);
            return SaveReview();
        }

        public bool DeleteReview(Review review)
        {
            _reviewContext.Remove(review);
            return SaveReview();
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            _reviewContext.RemoveRange(reviews);
            return SaveReview();
        }

        public Book GetBookOfReviw(int reviewId)
        {
            var bookId = _reviewContext.Reviews.Where(rv => rv.Id == reviewId).Select(b => b.Book.Id).FirstOrDefault();
            return _reviewContext.Books.Where(b => b.Id == bookId).FirstOrDefault();
        }

        public Review GetReview(int reviewId)
        {
            return _reviewContext.Reviews.Where(rv => rv.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _reviewContext.Reviews.ToList();
        }

        public ICollection<Review> GetReviewsOfBook(int bookId)
        {
            return _reviewContext.Reviews.Where(b => b.Book.Id == bookId).ToList();
        }

        public bool isExists(int reviewId)
        {
            return _reviewContext.Reviews.Where(rv => rv.Id == reviewId).Any();
        }

        public bool SaveReview()
        {
            var saved = _reviewContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
            _reviewContext.Update(review);
            return SaveReview();
        }
    }
}
