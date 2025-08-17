namespace Waracle_Hotels.Models
{
    public class HotelRoomBooking
    {
        public Guid HotelRoomBookingID { get; set; }
        public Guid HotelRoomID { get; set; }
        public string? BookingReference { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
    }
}
