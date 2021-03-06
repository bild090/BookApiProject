using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.Models
{
    public class Reviewer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "First Name can't be more than 100 characters")]
        public String FirstName { get; set; }
        [Required]
        [MaxLength(200, ErrorMessage = "Last Name can't be more than 200 characters")]
        public String LastName { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
