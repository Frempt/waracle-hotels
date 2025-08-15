using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Waracle_Hotels.Models;

namespace Waracle_Hotels.Controllers
{
    [Route("api/Hotels")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly HotelContext _context;

        public HotelsController(HotelContext context)
        {
            _context = context;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels()
        {
            return await _context.Hotel.ToListAsync();
        }

        // GET: api/Hotels/5
        [HttpGet("{name}")]
        public async Task<ActionResult<List<Hotel>>> GetHotels(string name)
        {
            var hotels = await _context.Hotel.ToListAsync();
            List<Hotel> returnHotels = new List<Hotel>();

            foreach (Hotel h in hotels)
            {
                if (h.HotelName != null && h.HotelName.ToUpper().Contains(name.ToUpper()))
                {
                    returnHotels.Add(h);
                }
            }

            return returnHotels;
        }

        private bool HotelExists(Guid id)
        {
            return _context.Hotel.Any(e => e.HotelID == id);
        }
    }
}
