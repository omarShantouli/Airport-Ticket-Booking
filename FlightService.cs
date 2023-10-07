namespace Airport_Ticket_Booking
{
    public static class FlightService
    {
        public static List<Flight> Flights = new List<Flight>();

        // ==== Method to add a flight
        public static bool AddFlight(Flight flight)
        {
            if(flight == null)
                return false;
            Flights.Add(flight);
            return true;
        }

        // ==== Method to display all existing flights in the system
        public static bool GetFlights()
        {
            try
            {

                if (Flights.Count() != 0)
                    foreach (Flight flight in Flights)
                    {
                        Console.WriteLine($"Fligh Number: {flight.FlightNumber}\nFirst Class Price: {flight.FirstClassPrice}$\n" +
                            $"Business Price: {flight.BusinessPrice}$\nEconomy Price: {flight.EconomyPrice}$\n" +
                            $"First Class Remaining Seats: {flight.RemainingFirstClassSeats}\nBusiness Remaining Seats: {flight.RemainingBusinessSeats}\n" +
                            $"Economy Remaining Seats: {flight.RemainingEconomySeats}\n" +
                            $"Departure Country: {flight.DepartureCountry}\nDestination Country: {flight.DestinationCountry}\n" +
                            $"Departure Date: {flight.DepartureDate}\nDeparture Airport: {flight.DepartureAirport}\n" +
                            $"Arrival Airport : {flight.ArrivalAirport}\n");

                        if (flight.Passengers.Count() != 0)
                        {
                            Console.WriteLine("Passengers:");
                            foreach (var p in flight.Passengers)
                            {
                                string @class = (from b in Booking.Bookings
                                                 where b.PassengerId == p.PassengerId && b.FlightNumber == flight.FlightNumber
                                                 select b).Single().Class.ToString();
                                if (@class == "FirstClass") @class = @class.Insert(5, " ");
                                Console.WriteLine($"Passenger ID: {p.PassengerId} | Name: {p.PassengerName} | Phone Number: {p.PhoneNumber} | Seat: {@class}");
                            }
                        }

                        else
                            Console.WriteLine("No Passengers on This Flight!");
                        Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                    }
                else
                    Console.WriteLine("No Flights Yet!");
                return true;
            }
            catch(Exception ex) {
                Console.WriteLine(ex.ToString()); 
                return false;
            }
        }
    }
}
