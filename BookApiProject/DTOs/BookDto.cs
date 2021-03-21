using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public String Isbn { get; set; }
        public String Title { get; set; }
        public DateTime DatePublished { get; set; }
    }

    public class BookRatingDto
    {
        public int Rating { get; set; } 
    }
}
