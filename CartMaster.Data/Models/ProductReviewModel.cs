using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Data.Models
{
    public class ProductReviewModel
    {
        public int ReviewID { get; set; }
        public int ProductID { get; set; }
        public int UserID { get; set; }
        public string? Username { get; set; }
        public int Rating {  get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    public class AverageRatingModel
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public decimal AverageRating { get; set; }
    }
}
