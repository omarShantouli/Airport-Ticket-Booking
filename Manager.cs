using System.Globalization;

namespace Airport_Ticket_Booking
{
    public static class Manager // (singleton)
    {
        public static string ManagerName { get; set; } = "Omar Hantouli";
        public static List<string> Status = new List<string>();

        // === Method to import flights from a csv file
        public static bool ImportFlights(string filePath)
        {

            try
            {
                List<Flight> flights = new List<Flight>();
                using (StreamReader reader = new StreamReader(filePath))
                {
                    // Skip the header row if it exists
                    string? headerLine = reader.ReadLine();
                    string? temp;
                    while (!reader.EndOfStream)
                    {
                        string? dataLine = reader.ReadLine();

                        string[] values = dataLine!.Split(',');
                        decimal x;
                        bool isNumber = decimal.TryParse(values[0].Replace("$", ""), out x) && values[0] != null && values[0] != "";
                        //Console.WriteLine("==> " + values[0]);
                        DateTime t;
                        bool isValidTime = DateTime.TryParse(values[3], out t) && values[3] != null && values[3] != "";
                        isValidTime = t > DateTime.Now;
                        values[3] = t.ToString("MM/dd/yyyy hh:mm");

                        temp = "";
                        bool isValidToAdd = true;
                        if (isNumber) { temp += "Price\n\tNumber\n\tRequired\n"; }
                        else { temp += "Price\n\tNot Number\n\tRequired\n"; isValidToAdd = false; }
                        if (isValidTime) { temp += "Departure Date\n\tDate Time\n\t• Required\n"; }
                        else { temp += "Departure Date\n\tNot Valid\n\tRequired\n"; isValidToAdd = false; }
                        if (values[1] != null && values[1] != "") { temp += "Departure Country\n\tFree Text\n\tRequired\n"; }
                        else { temp += "Departure Country\n\t• Empty\n\t• Required\n"; isValidToAdd = false; }
                        if (values[2] != null && values[2] != "") { temp += "Destination Country\n\tFree Text\n\tRequired\n"; }
                        else { temp += "Destination Country\n\tEmpty\n\tRequired\n"; isValidToAdd = false; }
                        if (values[4] != null && values[4] != "") { temp += "Destination Airport\n\tFree Text\n\tRequired\n"; }
                        else { temp += "Destination Airport\n\tEmpty\n\tRequired\n"; isValidToAdd = false; }
                        if (values[5] != null && values[5] != "") { temp += "Arrival Airport\n\tFree Text\n\tRequired\n"; }
                        else { temp += "Arrival Airport\n\tEmpty\n\tRequired\n"; isValidToAdd = false; }
                        Console.WriteLine("====?>");

                        if (isValidToAdd)
                        {
                            flights.Add(new Flight(x + 1000, x + 500, x, values[1], values[2], values[3], values[4], values[5]));
                            temp += "## Row is added\n";
                        }
                        else
                        {
                            temp += "## Row is not added!";
                        }

                        Manager.Status.Add(temp);
                    }
                }

                foreach (string st in Manager.Status)
                {
                    Console.WriteLine(st);
                    Console.WriteLine("------------------");
                }

                // write the read file
                foreach (Flight flight in flights)
                {
                    string[] dataRow =
                    {
                        flight.EconomyPrice.ToString(),
                        flight.DepartureCountry,
                        flight.DestinationCountry,
                        flight.DepartureDate,
                        flight.DepartureAirport,
                        flight.ArrivalAirport
                    };
                    using (StreamWriter writer = new StreamWriter("C:\\Users\\hanto\\Desktop\\Flights.csv", true))
                    {
                        FileSystem.WriteCsvRow(writer, dataRow);
                    }
                }
                FileSystem.ReadFlightsFile("C:\\Users\\hanto\\Desktop\\Flights.csv");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error message: " + ex.Message);
                return false;
            }

        }

        // ==== Method to search about flights
        public static void SearchFlightManagerSide()
        {

            string? Class = "", flightNumber = "", firstClassPrice = "", ecoPrice = "", businessPrice = "",
                    departureCountry = "", destinationCountry = "", departureDate = "", departureAirport = "",
                    arrivalAirport = "", passengerName = "", passengerId = "";
            Console.WriteLine("fill the following fields to search a flight, you can leave any field empty:");

            Console.Write("Passenger Name: ");
            passengerName = Console.ReadLine();
            Console.Write("Passenger ID: ");
            passengerId = Console.ReadLine();
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

            int? fn2, pId2;
            decimal? fc2, b2, e2;
            int.TryParse(flightNumber, out int fn);
            decimal.TryParse(firstClassPrice, out decimal fc);
            decimal.TryParse(businessPrice, out decimal b);
            decimal.TryParse(ecoPrice, out decimal e);
            int.TryParse(passengerId, out int pId);

            if (fn == 0) fn2 = null; else fn2 = fn;
            if (fc == 0) fc2 = null; else fc2 = fc;
            if (b == 0) b2 = null; else b2 = b;
            if (e == 0) e2 = null; else e2 = e;
            if (pId == 0) pId2 = null; else pId2 = pId;

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
            flights = flights.Where(f => departureDate == "" || f.DepartureDate.ToLower() == departureDate!.ToLower()).ToList();
            flights = flights.Where(f => departureAirport == "" || f.DepartureAirport.ToLower() == departureAirport!.ToLower()).ToList();
            flights = flights.Where(f => arrivalAirport == "" || f.ArrivalAirport.ToLower() == arrivalAirport!.ToLower()).ToList();

            flights = flights.Where(f => passengerName == "" ||
                                    f.Passengers.Where(p => p.PassengerName.ToLower() == passengerName!.ToLower()).Any()).ToList();
            flights = flights.Where(f => pId2 == null ||
                                    f.Passengers.Where(p => p.PassengerId == pId).Any()).ToList();

            if (flights.Count > 0)
            {
                Console.WriteLine("\n====================================================================");
                foreach (Flight flight in flights)
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
                            string @class = (from booking in Booking.Bookings
                                             where booking.PassengerId == p.PassengerId && booking.FlightNumber == flight.FlightNumber
                                             select booking).Single().Class.ToString();
                            if (@class == "FirstClass") @class = @class.Insert(5, " ");
                            Console.WriteLine($"Passenger ID: {p.PassengerId} | Name: {p.PassengerName} | Phone Number: {p.PhoneNumber} | Seat: {@class}");
                        }
                    }

                    else
                        Console.WriteLine("No Passengers on This Flight!");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                }
                Console.WriteLine("====================================================================");
                return;
            }
            Console.WriteLine("Oops! There is no any flight meets your search.");
            return;

        }
    }
}
