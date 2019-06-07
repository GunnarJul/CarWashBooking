using CarWashBooking.Models.DataModels;
using System.Collections.Generic;

namespace CarWashBooking.ViewModels
{
    public class CarWashViewModel
    {
        public string Search { get; set; }
        public List<CarWash> CarWashes { get; set; }
    }
}
