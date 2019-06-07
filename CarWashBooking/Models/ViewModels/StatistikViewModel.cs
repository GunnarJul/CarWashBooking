using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWashBooking.ViewModels
{
    public class StatistikViewModel
    {
       public DateTime FromDate { get; set; }
       public DateTime ToDate { get; set; }
       public Dictionary<string, int> Result;
    }

}
