using CartMaster.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Business.IServices
{
    public interface IPaymentService
    {
        public string InsertPaymentDetails(PaymentModel payment);
        public PaymentModel GetPaymentDetailsByOrderId(int orderId);
    }
}
