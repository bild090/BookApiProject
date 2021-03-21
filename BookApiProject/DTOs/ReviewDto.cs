using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public String Headline { get; set; }
        public String ReviewText { get; set; }
        public int Rating { get; set; }
    }
}
