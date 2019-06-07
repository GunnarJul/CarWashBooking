using CarWashBooking.DB;
using CarWashBooking.Models.DataModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CarWashBooking.Services
{
    public class CarWashBookingService : ICarWashBookingService
    {
        private DbContextOptions<CarWashBookingDbContext> _context;


        public CarWashBookingService(DbContextOptions<CarWashBookingDbContext> context)
        {
            _context = context;
        }

        public async  Task<List<WashBooking> > GetAllBookings(int year , int month, int washHallID)
        {
            using (var db = new CarWashBookingDbContext(_context))
            {
               return await  Task.FromResult(db.WashBookings.Where(w => w.Booking.Year == year && w.Booking.Month == month).ToList());
            }
        }

        public async Task<List<WashBooking>> GetUserBookings(Guid userID)
        {
            var now = DateTime.Now;
            using (var db = new CarWashBookingDbContext(_context))
            {
                // TODO >= now. Er det ikke forretningslogik! overfør som argument.
                return await Task.FromResult(db.WashBookings.Where(w => w.UserID == userID && w.Booking >= now).ToList());
            }
        }

        
        public  void AddCarWash(CarWash carWash)
        {
            using (var db = new CarWashBookingDbContext(_context))
            {
                 db.CarWashes.Add(carWash);
                
                db.SaveChanges();
               
            }
        }
        public async  Task<List<CarWash>> CarWashGetAll()
        {
            using (var db = new CarWashBookingDbContext(_context))
            {
                return await  Task.FromResult(db.CarWashes.ToList());
            }
        }

        public async Task<List<CarWash>> CarWashGetAll(string search)
        {
            using (var db = new CarWashBookingDbContext(_context))
            {
                return await Task.FromResult(db.CarWashes.Where (w => w.Name.Contains(search)) .ToList());
            }
        }

        public async Task<CarWash> ReadCarWash(int ID)
        {
            using (var db = new CarWashBookingDbContext(_context))
            {
                return await Task.FromResult(db.CarWashes.Where (w=> w.ID== ID).FirstOrDefault() );
            }
        }

        public async Task SaveCarWash(CarWash carWash)
        {
            using (var db = new CarWashBookingDbContext(_context))
            {
                var result = await Task.FromResult(db.CarWashes.Where(w => w.ID == carWash.ID).FirstOrDefault());
                if (result != null)
                {
                    result.Adresse = carWash.Adresse;
                    result.Name = carWash.Name ;
                    result.Image = carWash.Image;
                    db.SaveChanges();
                }
            }
        }

        public async Task   DeleteCarWash(CarWash carWash)
        {
            using (var db = new CarWashBookingDbContext(_context))
            {
               var result= await Task.FromResult(db.CarWashes.Remove(carWash));
               var saved = await Task.FromResult(db.SaveChanges());
            }
        }



        public WashBooking AddBooking(WashBooking booking)
        {
            using (var db = new CarWashBookingDbContext(_context))
            {
                db.WashBookings.Add(booking);
                db.SaveChanges();
                return booking;
            }
        }

        public async Task< List<DateTime>> Reserved(int carWashID, DateTime AtDate)
        {
            using (var db = new CarWashBookingDbContext(_context))
            {
                return await Task.FromResult(db.WashBookings.Where(w => w.CarWashID== carWashID && w.Booking.Date == AtDate.Date ).Select(s => s.Booking).ToList());
            }
        }

        public async Task<Dictionary <int, string>> AvailibleTimes(int carWashID, DateTime AtDate)
        {
            using (var db = new CarWashBookingDbContext(_context))
            {
                  
                var result= await Task.FromResult(db.WashBookings.Where(w => w.CarWashID == carWashID && w.Booking.Date == AtDate.Date).Select(s => s.Booking).ToList());
                var availibleMinutes =new Dictionary<int, string >();
                var selectedMin = new List<int>();
                result.ForEach(f => selectedMin.Add(f.Hour * 60 + f.Minute));
                // TODO  forretningslogik,! Husk at flytte !!!
                for (int hour = 0; hour < 24; hour++)
                {
                    for (int min = 0; min < 60; min += 20)
                    {
                        var val = hour * 60 + min;
                        if (selectedMin.Contains(val))
                        {
                            continue;
                        }

                        availibleMinutes.Add(val,  $"{hour}:{min:d2}");
                      
                    }
                }
                return availibleMinutes;
    }
        }

        public async Task<CarWash> LatestCarWash(Guid UserID)
        {

            using (var db = new CarWashBookingDbContext(_context))
            {
                var result = await Task.FromResult ( db.WashBookings.Where(w => w.UserID == UserID).OrderByDescending(o => o.Booking).FirstOrDefault());
                return result?.CarWash?? null;
            }
        }

        public async Task<Dictionary<int, string>> SearchCarWash(string search)
        {
            using (var db = new CarWashBookingDbContext(_context))
            {
                var searchResult = new Dictionary<int, string>();
                var result = await Task.FromResult(db.CarWashes.Where(w => w.Name.StartsWith(search)).Select(s => new { s.ID, s.Name }).Take(5).ToList());
                foreach (var item in result)
                {
                    searchResult.Add(item.ID, item.Name);
                }
                return searchResult;
            }
        }

        public async Task<Dictionary<string,int>> GetStatistik(DateTime FromDate, DateTime ToDate)
        {
            using (var db = new CarWashBookingDbContext(_context))
            {
                var result= await Task.FromResult(db.WashBookings.Where(w => w.Booking >= FromDate && w.Booking <= ToDate).GroupBy(g => g.CarWash.Name).ToDictionary(g => g.Key, g => g.Count()));
                return result;
            }
        }
    }
}
