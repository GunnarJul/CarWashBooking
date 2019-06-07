using System;
using System.Linq;
using System.Collections.Generic;
using CarWashBooking.Constants;

using CarWashBooking.Services;
using CarWashBooking.ViewModels;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CarWashBooking.Controllers
{
    public class HomeController : Controller
    {
        ICarWashBookingService _carWashBookingService;
      

        public HomeController(ICarWashBookingService service)
        {
            _carWashBookingService = service;
        }
        public async Task<IActionResult> Index()
        {
            var bookings =  new BookingListViewModel { UserName = User.Identity.Name, Bookings = null };
                
            if (User.Identity.IsAuthenticated)
            { 
                if(Guid.TryParse (Request.HttpContext.Session.GetString(SessionConstants.SessionActiceUserID),out var userID) )
                {
                    var washBookingList = await _carWashBookingService.GetUserBookings(userID);
                     foreach (var item in washBookingList)
                        item.CarWash = await _carWashBookingService.ReadCarWash(item.CarWashID);
                    var bookingLst = new List<BookingViewModel>();

                    washBookingList.ForEach(f => bookingLst.Add(new BookingViewModel { SelectedCarWash = f.CarWash, SelectedDate = f.Booking, UserName = User.Identity.Name } ));
                    bookings.Bookings = bookingLst.OrderBy(o => o.SelectedDate).ToList ();
                }
            }
            
            return View(bookings);
        }

        // https://www.devtrends.co.uk/blog/handling-404-not-found-in-asp.net-core
        [Route("error/404")]
        public IActionResult CatchAll()
        {
            Response.StatusCode = 404;
            return View("Index", "Home");
        }



    }
}