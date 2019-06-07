using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWashBooking.Services
{
    public interface IEmailService
    {
        Task SendEmail(string emailFrom, string emailTo, string subject, string message);
    }
}
