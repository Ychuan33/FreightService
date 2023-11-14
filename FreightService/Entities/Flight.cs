namespace FreightService.Entities
{
    public class Flight
    {
        public int FlightNumber { get; set; }
        public Airport Departure { get; set; }
        public Airport Arrival { get; set; }
        public int Day { get; set; }
        public int Capacity { get; set; } = 20;
        public int Scheduled { get; set; } = 0;
    }
}