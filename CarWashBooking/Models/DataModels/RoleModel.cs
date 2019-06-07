using System;
using Microsoft.AspNetCore.Identity;

namespace CarWashBooking.Models.DataModels
{
    public class RoleModel:IdentityRole<Guid>
    {
        public RoleModel()
        {
                
        }
        public RoleModel(string roleName):base (roleName)
        {

        }
    }
}
