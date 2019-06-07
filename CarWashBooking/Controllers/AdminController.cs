using System;
using System.Threading.Tasks;
using CarWashBooking.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using CarWashBooking.Models.DataModels;
using CarWashBooking.ViewModels;


namespace CarWashBooking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        ICarWashBookingService _carWashBookingService;
        public AdminController(ICarWashBookingService  service)
        {
            _carWashBookingService = service;
        }

        public async Task<IActionResult> Index()
        {
            var halls = await _carWashBookingService.CarWashGetAll();
            var carWashViewModel = new CarWashViewModel { Search = "", CarWashes = halls };
            return View(carWashViewModel);
        }

        public IActionResult UserOnline()
        {
            return View(nameof(UserOnline));
        }


        [HttpGet]
        public async Task<IActionResult> Search(string Search)
        {
            var search = string.IsNullOrEmpty(Search) ?"": Search.Trim();
            var halls =(string.IsNullOrEmpty (search)) ? await  _carWashBookingService.CarWashGetAll() : await _carWashBookingService.CarWashGetAll(search);
            var carWashViewModel = new CarWashViewModel { Search = search, CarWashes = halls };
           return View("Index", carWashViewModel);
        }

        public IActionResult NewCarWash()
        {
            return View();
        }

        public  IActionResult AddCarWash(NewCarWashViewModel carWashModel)
        {
            string base64;
            using (var ms = new MemoryStream())
            {
                carWashModel.UploadFile.CopyTo(ms);
                var fileBytes = ms.ToArray();
                base64 = $"data:image/png;base64,{Convert.ToBase64String(fileBytes)}";
            }  
            var carWash = new CarWash { Adresse = carWashModel.Adresse, Image = base64, Name = carWashModel.Name };

            _carWashBookingService.AddCarWash(carWash);
            return RedirectToAction("Index", "Admin");
        }

        public IActionResult SaveCarWash(CarWash carWash)
        {
            _carWashBookingService.SaveCarWash(carWash);
            return RedirectToAction("Index", "Admin");
        }

        public IActionResult DeleteCarWash(CarWash carWash)
        {
            _carWashBookingService.DeleteCarWash (carWash);
            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> EditCarWash(int ID)
        {
           var  carWash= await _carWashBookingService.ReadCarWash(ID);
            return View("EditCarWash", carWash);
        }
        public async Task< IActionResult> Statistik()
        {
            var date = DateTime.Now.Date;
            var fromDate = date.AddMonths(-1);
            var toDate = date;

            var dic = await _carWashBookingService.GetStatistik(fromDate,toDate );

            var statistik = new StatistikViewModel
            {
                FromDate = fromDate,    
                ToDate = toDate,
                Result = dic
            };
            return View(nameof(Statistik), statistik );
        }

        public async Task<IActionResult> Update(StatistikViewModel statistik)
        {

            statistik.Result = await _carWashBookingService.GetStatistik(statistik.FromDate , statistik.ToDate );
            return View(nameof(Statistik), statistik);
        }
    }
}