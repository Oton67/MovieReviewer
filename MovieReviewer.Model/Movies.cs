using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReviewer.Model
{
    public class Movies
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [ForeignKey(nameof(Genre))]
        public int? GenreID { get; set; }
        public Genre Genre { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name ="Relese Date")]
        public DateTime ReleaseDate { get; set; }
        [Required]
        [Range(1, 10, ErrorMessage = "Score must be between 1 and 10")]
        public double Score { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
