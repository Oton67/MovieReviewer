using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace MovieReviewer.Model
{
    public class Review
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        [Range(1, 10, ErrorMessage="Score must be between 1 and 10")]
        public double Score { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
        public string User { get; set; }
        [ForeignKey(nameof(Movie))]
        public int? MovieID { get; set; }
        public Movies Movie { get; set; }
    }
}
