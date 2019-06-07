using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CarWashBooking.Models.DataModels;

namespace CarWashBooking.DB
{
    public class CarWashBookingDbContext : DbContext
    {
        public DbSet<CarWash> CarWashes { get; set; }
        public DbSet<WashBooking> WashBookings{ get; set; }
        public DbSet<UserModel> UserModel { get; set; }

        public CarWashBookingDbContext(DbContextOptions<CarWashBookingDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvetion();
        }
    }

    public static class ModelBuilderExtensions
    {
        public static void RemovePluralizingTableNameConvetion(this ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }
        }
    }
}
