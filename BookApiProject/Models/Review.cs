using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Headline can't be more than 200 or less than 10 characters")]
        [Required]
        public String Headline { get; set; }
        [StringLength(2000, MinimumLength = 50, ErrorMessage = "Review Text can't be more than 2000 or less than 50 characters")]
        [Required]
        public String ReviewText { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage ="Rating must be between 1 and 5 stars")]
        public int Rating { get; set; }
        public virtual Book Book { get; set; }
        public virtual Reviewer Reviewer { get; set; }
    }
}
