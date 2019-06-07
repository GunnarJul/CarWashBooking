using Microsoft.AspNetCore.Http;

namespace CarWashBooking.ViewModels
{
    public class NewCarWashViewModel
    {
        
        public string Name { get; set; }
        public string Adresse { get; set; }
        public string Image { get; set; }
        public IFormFile UploadFile { get; set; }
    }
}
