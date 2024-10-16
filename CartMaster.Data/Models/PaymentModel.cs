using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Data.Models
{
    public class PaymentModel
    {
        public int PaymentID { get; set; }
        public int OrderID { get; set; }
        public string? PaymentMethod { get; set; }
        public string? CardName { get; set; }
        public string? CardNumber { get; set; }
        public string? ExpiryDate { get; set; }
        public string? CVV { get; set; }
        public string? UpiId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? PaymentStatus { get; set; }
    }
}
