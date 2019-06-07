using CarWashBooking.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace CarWashBooking.ViewComponents
{

    public class BookingViewComponent : ViewComponent
    {

        public BookingViewComponent()
        {

        }

        public IViewComponentResult Invoke(BookingViewModel booking)
        {
            return View("Default", booking);
        }
    }
}
