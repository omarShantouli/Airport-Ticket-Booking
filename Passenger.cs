using System.Globalization;

namespace Airport_Ticket_Booking
{
    public class Passenger
    {
        public static int LastId = 0;
        public int PassengerId { get; set; }
        public string PassengerName { get; set; }
        public string PhoneNumber { get; set; }

        public static List<Passenger> passengers = new List<Passenger>();

        // ==== Constructor
        public Passenger(string passengerName, string phoneNumber)
        {
            PassengerId = ++LastId;
            PassengerName = passengerName;
            PhoneNumber = phoneNumber;
            passengers.Add(this);
        }

        // ==== Method to book a flight
        public bool BookFlight(int flightId, FlightClass @class)
        {
            try
            {
                Booking booking = new Booking(this.PassengerId, flightId, @class);
                Booking.Bookings.Add(booking);
                string[] dataRow = {
                booking.BookingId.ToString(),
                booking.PassengerId.ToString(),
                booking.FlightNumber.ToString(),
                booking.DepartureDate.ToString(),
                booking.Class.ToString(),
                booking.Price.ToString(),
                };
                using (StreamWriter writer = new StreamWriter("C:\\Users\\hanto\\Desktop\\bookings.csv", true))
                {
                    FileSystem.WriteCsvRow(writer, dataRow);
                }
                return true;
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Error Message: {ex}");
                return false;
            }
        }

        // ==== Method to cancel a booked flight if exist
        public bool CancelFlight(int flightId)
        {
            try
            {
                Flight f = FlightService.Flights.Where(f => f.FlightNumber == flightId).Single();
                f.Passengers.RemoveAll(p => p.PassengerId == this.PassengerId);
                f.CancelPassenger((from b in Booking.Bookings where b.FlightNumber == flightId && b.PassengerId == this.PassengerId select b).Single().Class);
                Booking.Bookings = (from b in Booking.Bookings where b.FlightNumber != flightId && b.PassengerId != this.PassengerId select b).ToList();

                File.WriteAllText("C:\\Users\\hanto\\Desktop\\bookings.csv", string.Empty);

                foreach (Booking booking in Booking.Bookings)
                {
                    string[] dataRow = {
                booking.BookingId.ToString(),
                booking.PassengerId.ToString(),
                booking.FlightNumber.ToString(),
                booking.DepartureDate.ToString(),
                booking.Class.ToString(),
                booking.Price.ToString(),
                };

                    using (StreamWriter writer = new StreamWriter("C:\\Users\\hanto\\Desktop\\bookings.csv"))
                    {
                        // Write header 
                        writer.WriteLine("Booking ID,Passenger ID,Flight Number,Departure Date,Class,Price");
                        FileSystem.WriteCsvRow(writer, dataRow);
                    }
                }
                return true;
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Error Message: {ex}");
                return false;
            }

        }

        // ==== Method to display all booked Flights
        public bool GetBookings()
        {
            try
            {
                List<Booking> bookings = new List<Booking>();
                bookings = (from b in Booking.Bookings where b.PassengerId == this.PassengerId select b).ToList();

                Passenger passenger = (from p in Passenger.passengers where p.PassengerId == this.PassengerId select p).Single();
                Console.WriteLine("======================================================================================");
                Console.WriteLine($"\nPassenger ID: {passenger.PassengerId} | Name: {passenger.PassengerName} | Phone Number: {passenger.PhoneNumber}");
                Console.WriteLine("Booked Flights: \n");
                if (bookings.Count > 0)
                {
                    foreach (Booking booking in bookings)
                    {
                        Console.WriteLine($"Booking ID: {booking.BookingId}");
                        Flight flight = (from f in FlightService.Flights where f.FlightNumber == booking.FlightNumber select f).Single();

                        Console.WriteLine($"Fligh Number: {flight.FlightNumber}\nPrice: {booking.Price}$\n" +
                            $"Departure Country: {flight.DepartureCountry}\nDestination Country: {flight.DestinationCountry}\n" +
                            $"Departure Date: {flight.DepartureDate}\nDeparture Airport: {flight.DepartureAirport}\n" +
                            $"Arrival Airport : {flight.ArrivalAirport}\n" +
                            $"Class: {booking.Class}");
                        Console.WriteLine("----------------------------\n");
                    }
                }
                else
                {
                    Console.WriteLine($"Hey {passenger.PassengerName}! You did not book any Flight yet!");
                }
                Console.WriteLine("======================================================================================");
                return true;
            }
            catch(Exception ex) {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        // ==== Method to display all passengers in the system
        public static bool GetPassengers()
        {
            try
            {
                if (Passenger.passengers.Count() == 0)
                {
                    Console.WriteLine("There are no passengers!");
                }
                else foreach (var p in Passenger.passengers)
                    {
                        Console.WriteLine($"Passenger ID: {p.PassengerId} | Name: {p.PassengerName} | Phone Number: {p.PhoneNumber}");
                    }
                return true;
            }
            catch (Exception ex) { Console.WriteLine(); return false; }
        }
        // ==== Method that overrides ToString() method
        public override string ToString()
        {
            return $"{this.PassengerName} {this.PhoneNumber} {this.PassengerId}";
        }
        //==== Method to search a flight
        public static bool SearchFlight()
        {
            try
            {
                string? Class = "", flightNumber = "", firstClassPrice = "", ecoPrice = "", businessPrice = "",
                        departureCountry = "", destinationCountry = "", departureDate = "", departureAirport = "",
                        arrivalAirport = "";
                Console.WriteLine("fill the following fields to search a flight, you can leave any field empty:");

                Console.Write("Class: ");
                Class = Console.ReadLine();
                TextInfo text = new CultureInfo("en-US").TextInfo;
                Class = text.ToTitleCase(Class ?? "");
                Class = Class.Replace(" ", "");
                Console.Write("Flight Number: ");
                flightNumber = Console.ReadLine();
                Console.Write("First Class Price: ");
                firstClassPrice = Console.ReadLine();
                Console.Write("Business Price: ");
                businessPrice = Console.ReadLine();
                Console.Write("Economy Price: ");
                ecoPrice = Console.ReadLine();
                Console.Write("Departure Country: ");
                departureCountry = Console.ReadLine();
                Console.Write("Destination Country: ");
                destinationCountry = Console.ReadLine();
                Console.Write("Departure Date: (mm/dd/yyyy hh:mm) ");
                departureDate = Console.ReadLine();
                Console.Write("Departure Airport: ");
                departureAirport = Console.ReadLine();
                Console.Write("Arrival Airport: ");
                arrivalAirport = Console.ReadLine();

                int? fn2;
                decimal? fc2, b2, e2;
                int.TryParse(flightNumber, out int fn);
                decimal.TryParse(firstClassPrice, out decimal fc);
                decimal.TryParse(businessPrice, out decimal b);
                decimal.TryParse(ecoPrice, out decimal e);

                if (fn == 0) fn2 = null; else fn2 = fn;
                if (fc == 0) fc2 = null; else fc2 = fc;
                if (b == 0) b2 = null; else b2 = b;
                if (e == 0) e2 = null; else e2 = e;


                DateTime t;
                if (departureDate != "")
                {
                    DateTime.TryParse(departureDate, out t);
                    departureDate = t.ToString("MM/dd/yyyy hh:mm");
                }
                var flights = FlightService.Flights.Where(f => fn2 == null || f.FlightNumber == fn2).ToList();
                if (Class != "")
                {
                    if (Class == "FirstClass")
                        flights = flights.Where(f => f.RemainingFirstClassSeats > 0).ToList();
                    else if (Class == "Economy")
                        flights = flights.Where(f => f.RemainingEconomySeats > 0).ToList();
                    else if (Class == "Business")
                        flights = flights.Where(f => f.RemainingBusinessSeats > 0).ToList();
                    else flights = new List<Flight>();
                }

                flights = flights.Where(f => fc2 == null || f.FirstClassPrice == fc2).ToList();
                flights = flights.Where(f => b2 == null || f.BusinessPrice == b2).ToList();
                flights = flights.Where(f => e2 == null || f.EconomyPrice == e2).ToList();
                flights = flights.Where(f => departureCountry == "" || f.DepartureCountry.ToLower() == departureCountry!.ToLower()).ToList();
                flights = flights.Where(f => destinationCountry == "" || f.DestinationCountry.ToLower() == destinationCountry!.ToLower()).ToList();
                flights = flights.Where(f => departureDate == "" || f.DepartureDate == departureDate).ToList();
                if (departureAirport != null)
                    flights = flights.Where(f => departureAirport == "" || f.DepartureAirport.ToLower() == departureAirport.ToLower()).ToList();
                flights = flights.Where(f => arrivalAirport == "" || f.ArrivalAirport.ToLower() == arrivalAirport!.ToLower()).ToList();


                if (flights != null && flights.Count > 0)
                {
                    Console.WriteLine("\n====================================================================");
                    foreach (Flight flight in flights)
                    {
                        Console.WriteLine($"Fligh Number: {flight.FlightNumber}\n" +
                            $"First Class Price: {flight.FirstClassPrice}\n" +
                            $"Business Price: {flight.BusinessPrice}\n" +
                            $"Economy Price: {flight.EconomyPrice}\n" +
                            $"First Class Remaining Seats: {flight.RemainingFirstClassSeats}\n" +
                            $"Business Remaining Seats: {flight.RemainingBusinessSeats}\n" +
                            $"Economy Remaining Seats: {flight.RemainingEconomySeats}\n" +
                            $"Departure Country: {flight.DepartureCountry}\n" +
                            $"Destination Country: {flight.DestinationCountry}\n" +
                            $"Departure Date: {flight.DepartureDate}\n" +
                            $"Departure Airport: {flight.DepartureAirport}\n" +
                            $"Arrival Airport : {flight.ArrivalAirport}");
                        Console.WriteLine("-------------------------------------------------");
                    }
                    Console.WriteLine("====================================================================");

                }
                else
                {
                    Console.WriteLine("Oops! There is no any flight meets your search.");
                }
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            }
        // ====
    }
}
