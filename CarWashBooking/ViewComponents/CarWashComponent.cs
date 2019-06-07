using CarWashBooking.Models.DataModels;
using Microsoft.AspNetCore.Mvc;


namespace CarWashBooking.ViewComponents
{
    public class CarWashViewComponent : ViewComponent
    {
        public CarWashViewComponent()
        {

        }

        public IViewComponentResult Invoke(CarWash carWash)
        {
            return View("Default", carWash);
        }
    }
}
