using Microsoft.EntityFrameworkCore;

namespace Waracle_Hotels.Models
{
    public class HotelContext : DbContext
    {
        public HotelContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Hotel> Hotel { get; set; } = null!;
        public DbSet<HotelRoom> HotelRoom { get; set; } = null!;
        public DbSet<RoomType> RoomType { get; set; } = null!;
        public DbSet<HotelRoomBooking> HotelRoomBooking { get; set; } = null!;

    }
}
