using CarWashBooking.Models.DataModels;
using System;

using System.ComponentModel.DataAnnotations;


namespace CarWashBooking.ViewModels
{
    public class BookingViewModel
    {
        public CarWash SelectedCarWash { get; set; }

     
        public DateTime SelectedDate { get; set; }
        
        public string UserName { get; set; }
        [Display(Name = "Dato")]
        public string DisplayDate
        {
            get
            {
                return this.SelectedDate.ToShortDateString();
            }
        }
        [Display(Name = "Tid :")]
        public string DisplayTime
        {
            get
            {
                return this.SelectedDate.ToShortTimeString();
            }
        }
    }


}


