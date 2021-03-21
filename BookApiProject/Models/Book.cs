using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(10,MinimumLength = 3, ErrorMessage = "Isbn can't be more than 10 or less than 3 characters")]
        public String Isbn { get; set; }
        [Required]
        [MaxLength(200, ErrorMessage = "Title can't be more than 200 characters")]
        public String Title { get; set; }
        public DateTime DatePublished { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
        public virtual ICollection<BookCategory> BookCategories { get; set; }

}
}
