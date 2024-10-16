using CartMaster.Business.IServices;
using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Business.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public PaymentModel GetPaymentDetailsByOrderId(int orderId)
        {
            return _paymentRepository.GetPaymentDetailsByOrderId(orderId);
        }

        public string InsertPaymentDetails(PaymentModel paymentModel)
        {
            return _paymentRepository.InsertPaymentDetails(paymentModel);
        }
    }
}
