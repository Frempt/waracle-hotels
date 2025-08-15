using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Waracle_Hotels.Models;

namespace Waracle_Hotels.Controllers
{
    [Route("api/Rooms")]
    [ApiController]
    public class HotelRoomController : ControllerBase
    {
        private readonly HotelContext _context;

        public HotelRoomController(HotelContext context)
        {
            _context = context;
        }

        // GET: api/Rooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelRoom>>> GetHotelRooms()
        {
            return await _context.HotelRoom.ToListAsync();
        }

        //GET: api/Rooms/Availability
        [HttpGet("Availability")]
        public async Task<ActionResult<IEnumerable<HotelRoom>>> GetAvailableHotelRooms(string checkIn, string checkOut, int numberOfGuests)

        {
            var rooms = await _context.HotelRoom.ToListAsync();
            DateTime checkInDate, checkOutDate = DateTime.Now;

            if (!DateTime.TryParse(checkIn, out checkInDate)
                || !DateTime.TryParse(checkOut, out checkOutDate))
            {
                //todo throw error
            }

            List<HotelRoom> returnRooms = new List<HotelRoom>();
            List<HotelRoomBooking> allBookings = await _context.HotelRoomBooking.ToListAsync();

            foreach (HotelRoom room in rooms)
            {
                RoomType? roomType = _context.RoomType.Find(room.RoomTypeID);
                if (numberOfGuests <= roomType.Capacity)
                {
                    IEnumerable<HotelRoomBooking> bookings = allBookings.Where(t => t.HotelRoomID == room.HotelRoomID
                                                                                                            && ((t.CheckInDate >= checkInDate && t.CheckInDate < checkOutDate)
                                                                                                            || (t.CheckInDate <= checkInDate && t.CheckOutDate >= checkInDate)));
                    if (bookings.Count() == 0)
                    {
                        returnRooms.Add(room);
                    }
                }
            }

            return returnRooms;
        }
    }
}
