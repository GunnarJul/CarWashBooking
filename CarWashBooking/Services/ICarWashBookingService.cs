using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarWashBooking.Models.DataModels;

namespace CarWashBooking.Services
{
    public interface ICarWashBookingService
    {
        Task<List<WashBooking>> GetAllBookings(int year, int month, int washHall);

        Task<List<WashBooking>> GetUserBookings(Guid userID);

        Task<List<CarWash>> CarWashGetAll ();

        Task<List<CarWash>> CarWashGetAll(string search);


        Task<CarWash> ReadCarWash(int ID);
        void AddCarWash(CarWash carWash);
        Task SaveCarWash(CarWash carWash);
        Task DeleteCarWash(CarWash carWash);

        Task<Dictionary<int,string>>  SearchCarWash(string search );
        Task<List<DateTime>> Reserved(int carWashID, DateTime AtDate);
        Task<Dictionary<int, string>> AvailibleTimes(int carWashID, DateTime AtDate);

        Task<Dictionary<string,int>> GetStatistik(DateTime FromDate, DateTime DateTimeToDate);


        WashBooking AddBooking(WashBooking booking);

        Task<CarWash> LatestCarWash(Guid UserID);
    }
}