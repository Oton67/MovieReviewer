using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReviewer.Models
{
    public class ReviewFilterModel
    {
        public string User { get; set; }
        public string Title { get; set; }
        [Range(1, 10)]
        public int Score { get; set; }
        public int MovieID { get; set; }
    }
}
