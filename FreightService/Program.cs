using FreightService;

var flightManager = new FlightManager();
flightManager.LoadDefaultFlights();
flightManager.DisplayFlightSchedule();

flightManager.LoadOrders("coding-assigment-orders.json");
flightManager.ScheduleFlights();
flightManager.DisplayOrders();