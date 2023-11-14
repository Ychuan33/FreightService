using FreightService.Entities;
using Newtonsoft.Json;

namespace FreightService
{
    public class FlightManager
    {
        private Dictionary<Airport, Dictionary<int, List<Flight>>> FlightSchedule = new();
        private List<Order> Orders = new();

        public void LoadDefaultFlights()
        {
            AddFlight(new Flight { FlightNumber = 1, Departure = Airport.YUL, Arrival = Airport.YYZ, Day = 1 });
            AddFlight(new Flight { FlightNumber = 2, Departure = Airport.YUL, Arrival = Airport.YYC, Day = 1 });
            AddFlight(new Flight { FlightNumber = 3, Departure = Airport.YUL, Arrival = Airport.YVR, Day = 1 });
            AddFlight(new Flight { FlightNumber = 4, Departure = Airport.YUL, Arrival = Airport.YYZ, Day = 2 });
            AddFlight(new Flight { FlightNumber = 5, Departure = Airport.YUL, Arrival = Airport.YYC, Day = 2 });
            AddFlight(new Flight { FlightNumber = 6, Departure = Airport.YUL, Arrival = Airport.YVR, Day = 2 });
        }

        public void AddFlight(Flight flight)
        {
            if (!FlightSchedule.ContainsKey(flight.Arrival))
            {
                FlightSchedule[flight.Arrival] = new Dictionary<int, List<Flight>>();
            }

            if (!FlightSchedule[flight.Arrival].ContainsKey(flight.Day))
            {
                FlightSchedule[flight.Arrival][flight.Day] = new List<Flight>();
            }

            FlightSchedule[flight.Arrival][flight.Day].Add(flight);
        }

        public void DisplayFlightSchedule()
        {
            var allFlights = FlightSchedule
                .SelectMany(flights => flights.Value.Values.SelectMany(dayFlights => dayFlights))
                .OrderBy(flight => flight.FlightNumber)
                .ToList();

            if (allFlights.Count > 0)
            {
                Console.WriteLine("All flights:");

                foreach (var flight in allFlights)
                {
                    Console.WriteLine($"Flight: {flight.FlightNumber}, Departure: {flight.Departure}, Arrival: {flight.Arrival}, Day: {flight.Day}");
                }
            }
            else
            {
                Console.WriteLine("No flights found");
            }
        }

        public void LoadOrders(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var parsedJson = JsonConvert.DeserializeObject<Dictionary<string, Order>>(json);
            if (parsedJson != null)
            {
                Orders = parsedJson.Select(item =>
                {
                    item.Value.Name = item.Key;
                    return item.Value;
                }).ToList();
            }
        }

        public void ScheduleFlights()
        {
            Orders.Select(order =>
            {
                var flight = FindAvailableFlight(order.Arrival);
                if (flight != null)
                {
                    order.FlightNumber = flight.FlightNumber;
                    order.Day = flight.Day;
                }
                return order;
            }).ToList();
        }

        private Flight? FindAvailableFlight(Airport arrival)
        {
            if (FlightSchedule.TryGetValue(arrival, out var scheduleByCity))
            {
                var sortedDays = scheduleByCity.Keys.OrderBy(day => day).ToList();

                foreach (var day in sortedDays)
                {
                    foreach (var flight in scheduleByCity[day])
                    {
                        if (flight.Scheduled < flight.Capacity)
                        {
                            ++flight.Scheduled;
                            return flight;
                        }
                    }
                }
            }
            return null;
        }

        public void DisplayOrders()
        {
            Console.WriteLine("All orders:");

            foreach (var order in Orders)
            {
                if (order.Day == null || order.FlightNumber == null)
                {
                    Console.WriteLine($"Order ID: {order.Name}, FlightNumber: not scheduled");
                }
                else
                {
                    Console.WriteLine($"Order ID: {order.Name}, FlightNumber: {order.FlightNumber}, Departure: {order.Departure}, Arrival: {order.Arrival}, Day: {order.Day}");
                }
            }
        }
    }
}