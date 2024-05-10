using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.IServices
{
    public interface IEmailService
    {
        public void SendEmail(string recipient, string subject, string body);
    }
}
