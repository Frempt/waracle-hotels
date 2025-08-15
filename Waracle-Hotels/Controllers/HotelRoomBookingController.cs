using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Waracle_Hotels.Models;

namespace Waracle_Hotels.Controllers
{
    [Route("api/Bookings")]
    [ApiController]
    public class HotelRoomBookingController : ControllerBase
    {
        private readonly HotelContext _context;

        public HotelRoomBookingController(HotelContext context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelRoomBooking>>> GetHotelRoomBookings()
        {
            return await _context.HotelRoomBooking.ToListAsync();
        }

        [HttpGet("{reference}")]
        [ActionName(nameof(GetHotelRoomBooking))]
        public async Task<ActionResult<HotelRoomBooking>> GetHotelRoomBooking(string reference)
        {
            var bookings = await _context.HotelRoomBooking.ToListAsync();

            foreach (HotelRoomBooking b in bookings)
            {
                if (b.BookingReference != null && b.BookingReference.ToUpper().Contains(reference.ToUpper()))
                {
                    return b;
                }
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<HotelRoomBooking>> CreateHotelRoomBooking([FromBody] HotelRoomBooking request)
        {

            HotelRoom room = _context.HotelRoom.Find(request.HotelRoomID);
            RoomType roomType = _context.RoomType.Find(room.RoomTypeID);

            if (room == null || roomType == null)// || roomType.Capacity < request.numberOfGuests) //todo - need to know how many guests in this booking to validate
            {
                //todo validation failed, do not proceed
            }
            else
            {
                HotelRoomBooking hotelRoomBooking = new HotelRoomBooking();
                hotelRoomBooking.CheckInDate = request.CheckInDate;
                hotelRoomBooking.CheckOutDate = request.CheckOutDate;
                hotelRoomBooking.HotelRoomID = room.HotelRoomID;
                string? maxReference = "";
                maxReference = _context.HotelRoomBooking.OrderByDescending(t => t.BookingReference).First().BookingReference;

                hotelRoomBooking.BookingReference = (Int32.Parse(maxReference) + 1).ToString("0000000");

                _context.HotelRoomBooking.Add(hotelRoomBooking);
                await _context.SaveChangesAsync();

                return Ok(hotelRoomBooking.BookingReference);
            }

            return BadRequest();
        }
    }
}
