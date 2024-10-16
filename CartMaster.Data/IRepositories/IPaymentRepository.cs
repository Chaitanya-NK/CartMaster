using CartMaster.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Data.IRepositories
{
    public interface IPaymentRepository
    {
        public string InsertPaymentDetails(PaymentModel payment);
        public PaymentModel GetPaymentDetailsByOrderId(int orderId);
    }
}
