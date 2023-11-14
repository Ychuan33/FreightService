using Newtonsoft.Json;

namespace FreightService.Entities
{
    public class Order
    {
        public string? Name { get; set; }

        public Airport Departure { get; set; } = Airport.YUL;

        [JsonProperty("destination")]
        public Airport Arrival { get; set; }

        public int? FlightNumber { get; set; }

        public int? Day { get; set; }
    }
}