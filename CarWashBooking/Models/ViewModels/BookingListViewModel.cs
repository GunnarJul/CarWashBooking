using System.Collections.Generic;
 

namespace CarWashBooking.ViewModels
{
    public class BookingListViewModel
    {   public string UserName { get; set; }
        public List<BookingViewModel> Bookings { get; set; }
    }
}
