 using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using CarWashBooking.Constants;
using CarWashBooking.Services;
using CarWashBooking.ViewModels;

using CarWashBooking.Models.DataModels;
using Microsoft.AspNetCore.Identity;

namespace CarWashBooking.Controllers
{
    public class BookingController : Controller
    {
        ICarWashBookingService _carWashBookingService;
        private IEmailService _emailService;


        public BookingController(ICarWashBookingService service, IEmailService emailService)
        {
            _carWashBookingService = service;
            _emailService = emailService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            DateTime selectDate = DateTime.Now;
            int selectedTime = selectDate.Hour * 60;
            int min = 0;
            if (selectDate.Minute < 20)
                min = 20;
            else if (selectDate.Minute < 40)
                min = 40;
            else
                min = 60;
            selectedTime += min;
            
            var userId = Request.HttpContext.Session.GetString(SessionConstants.SessionActiceUserID);

            // hent brugers sidste hal og vask.
            var washHalls = await _carWashBookingService.CarWashGetAll();

            if (washHalls.Count == 0)
            {
               return  View("NoWashHall");
            }
            var latest = await _carWashBookingService.LatestCarWash(Guid.Parse(userId));
            
            // for meget logik her
            var washHall = (latest== null) ? washHalls[0]: latest;

            var reserved = await _carWashBookingService.Reserved(washHall.ID, selectDate);
            var availibleTimes = _carWashBookingService.AvailibleTimes(washHall.ID, selectDate);
            int selectDateNext = availibleTimes.Result.Keys.FirstOrDefault(w => selectedTime >= w);

            if (selectDateNext > 0)
                selectedTime = selectDateNext;

            var booking = new NewBookingViewModel
            {
                SelectedCarWash = washHall,
                CarWashes = washHalls,
                SelectedDate = selectDate,
                SelectedCarWashID = washHall.ID,
                SelectTime=selectedTime,
                AvailibleTimes= availibleTimes.Result,
                UserName= User.Identity.Name 
            };
           
            return View("Index", booking);
        }

        private void SendEmail(NewBookingViewModel booking)
        {
            string message = $"Hej \nHusk din bilvask {booking.DisplayDate }  {booking.DisplayTime} på adresse {booking.SelectedCarWash.Adresse }";
                _emailService.SendEmail("noreply@bilvask.dk",User.Identity.Name , "Booking af bilvask", message).Wait();
        }

        //  https://www.c-sharpcorner.com/blogs/drop-down-list-selected-index-changed-event-in-mvc

        public async Task<PartialViewResult> ReadCarWash(int selectedID)
        {
            var carWash = await _carWashBookingService.ReadCarWash(selectedID);
            var booking = new NewBookingViewModel() { SelectedCarWash = carWash };
            return PartialView("_CarWashPartial", booking);
        }
        [HttpPost]
        public IActionResult Reservation(NewBookingViewModel bookingModel)
        {
            var userId = Guid.Parse (Request.HttpContext.Session.GetString(SessionConstants.SessionActiceUserID));
            int hour =(int) bookingModel.SelectTime / 60;
            int min = (int)bookingModel.SelectTime - (hour * 60);

            var selectedDate = new DateTime(
                bookingModel.SelectedDate.Year,
                bookingModel.SelectedDate.Month, bookingModel.SelectedDate.Day, hour, min,0);

            if (selectedDate < DateTime.Now)
            {
               return RedirectToAction("Index","Home");
            }
             var booking = new WashBooking
            {
                Booking = selectedDate,
                UserID = userId,
                CarWashID = bookingModel.SelectedCarWashID,
            };
             _carWashBookingService.AddBooking(booking);
            bookingModel.SelectedCarWash = _carWashBookingService.ReadCarWash(bookingModel.SelectedCarWashID).Result ;
            bookingModel.SelectedDate = selectedDate;
            SendEmail (bookingModel);
            return RedirectToAction ("Index", "Home");
       
        }

        [HttpPost]
        public async Task<JsonResult> SearchCarWash(string search)
        {
            var carWashes = await _carWashBookingService.SearchCarWash(search );
            return Json(carWashes);
        }

    }
}