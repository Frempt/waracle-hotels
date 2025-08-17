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
        public async Task<ActionResult<HotelRoomBooking>> CreateHotelRoomBooking([FromBody] HotelRoomBookingRequest request)
        {
            HotelRoom room = _context.HotelRoom.Find(request.HotelRoomID);
            RoomType roomType = _context.RoomType.Find(room.RoomTypeID);
            IEnumerable<HotelRoomBooking> bookings = (await _context.HotelRoomBooking.ToListAsync()).Where(t => t.HotelRoomID == room.HotelRoomID
                                                                                                            && ((t.CheckInDate >= request.CheckInDate && t.CheckInDate < request.CheckOutDate)
                                                                                                            || (t.CheckInDate <= request.CheckInDate && t.CheckOutDate >= request.CheckInDate)));
            //ensure the room and room type exist, the guest count isn't over the room type's capacity (and there are guests), and the room isn't already booked during these dates
            if (room == null || roomType == null
                || request.NumberOfGuests > roomType.Capacity
                || request.NumberOfGuests == 0
                || bookings.Count() > 0
                || request.CheckOutDate <= request.CheckInDate)
            {
                return BadRequest();
            }
            else
            {
                HotelRoomBooking hotelRoomBooking = new HotelRoomBooking();
                hotelRoomBooking.CheckInDate = request.CheckInDate.Date;
                hotelRoomBooking.CheckOutDate = request.CheckOutDate.Date;
                hotelRoomBooking.HotelRoomID = room.HotelRoomID;
                hotelRoomBooking.NumberOfGuests = request.NumberOfGuests;
                string? maxReference = "";
                maxReference = _context.HotelRoomBooking.AsEnumerable().OrderByDescending(booking => int.Parse(booking.BookingReference)).First().BookingReference;
                if (maxReference == null) maxReference = "00000000";
                hotelRoomBooking.BookingReference = (int.Parse(maxReference) + 1).ToString("0000000");

                _context.HotelRoomBooking.Add(hotelRoomBooking);
                await _context.SaveChangesAsync();

                return Ok(hotelRoomBooking.BookingReference);
            }
        }

        [HttpPost("Reset")]
        public async Task<ActionResult<bool>> ResetDatabase()
        {
            try
            {
                FormattableString sql = $"EXEC DBReset";
                await _context.Database.ExecuteSqlAsync(sql);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        [HttpPost("Seed")]
        public async Task<ActionResult<bool>> SeedDatabase()
        {
            try
            {

                FormattableString sql = $"EXEC DBSeed";
                await _context.Database.ExecuteSqlAsync(sql);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
