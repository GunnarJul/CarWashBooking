using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CarWashBooking.Models.DataModels
{
    public class CarWash
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [DisplayName("Navn")]
        [Required]
        [MaxLength(10)]
        public string Name { get; set; }


        [Required]
        [MaxLength(50)]
        public string Adresse { get; set; }

        public string Image { get; set; }
        //[ForeignKey("CarWashID")]
        //public ICollection<WashBooking> Bookings { get; set; }
    }
}
