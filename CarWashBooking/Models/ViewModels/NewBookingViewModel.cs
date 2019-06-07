using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CarWashBooking.Models.DataModels;

namespace CarWashBooking.ViewModels
{
    public class NewBookingViewModel
    {
        [Display(Name = "Vælg vaskehal")]
        public int SelectedCarWashID { get; set; }

        public List<CarWash> CarWashes { get; set; }
        public CarWash SelectedCarWash { get; set; }

        [Display(Name = "Vælg en dag")]
        public DateTime SelectedDate { get; set; }
        [Display(Name = "Vælg tidspunkt")]
        public int SelectTime { get; set; }

        public Dictionary<int, string> AvailibleTimes { get; set; }
        public string UserName { get; set; }

        public string DisplayDate
        {
            get
            {
                return this.SelectedDate.ToShortDateString();
            }
        }
        public string DisplayTime
        {
            get
            {
                return this.SelectedDate.ToShortTimeString();
            }
        }
    }


}
