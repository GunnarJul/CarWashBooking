using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWashBooking.ViewModels
{
    public class ReservationViewModel
    {
        public string UserName { get; set; }
        public DateTime Booking { get; set; }
        public CarWashViewModel CarWashView {get;set;}
    }
}
