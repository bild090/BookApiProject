using BookApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.Services
{
    public class ReviewerRepository : IReviewerRepository
    {
        private BookDbContext _reviewerContext;
        public ReviewerRepository(BookDbContext reviewerContext)
        {
            _reviewerContext = reviewerContext;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _reviewerContext.Add(reviewer);
            return SaveReviewer();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _reviewerContext.Remove(reviewer);
            return SaveReviewer();
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return _reviewerContext.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefault();
        }

        public Reviewer GetReviewerOfReview(int reviewId)
        {
            var reviewerId =  _reviewerContext.Reviews.Where(r => r.Id == reviewId).Select(s => s.Reviewer.Id).FirstOrDefault();
            return _reviewerContext.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _reviewerContext.Reviewers.ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return _reviewerContext.Reviews.Where(rv => rv.Id == reviewerId).ToList();
        }

        public bool isExists(int reviewerId)
        {
            return _reviewerContext.Reviewers.Where(r => r.Id == reviewerId).Any();
        }

        public bool SaveReviewer()
        {
            var saved = _reviewerContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _reviewerContext.Update(reviewer);
            return SaveReviewer();
        }
    }
}
