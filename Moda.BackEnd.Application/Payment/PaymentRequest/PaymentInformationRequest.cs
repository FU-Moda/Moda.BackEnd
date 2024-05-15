using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.Payment.PaymentRequest
{
    public class PaymentInformationRequest
    {
        public string OrderID { get; set; }
        public string AccountID { get; set; } = null!;  
        public string CustomerName { get; set; } = null!;   
        public double Amount { get; set; }
    }
}
