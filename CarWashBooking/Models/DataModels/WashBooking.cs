using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWashBooking.Models.DataModels
{

    public class WashBooking
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public Guid UserID { get; set; }


        [ForeignKey("CarWashID")]
        public CarWash CarWash { get; set; }

        public int CarWashID { get; set; }
        
        

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime Booking { get; set; }


    }
}
