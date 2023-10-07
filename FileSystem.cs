namespace Airport_Ticket_Booking
{
    public static class FileSystem
    {

        //==== Method to read passengers file
        public static List<Passenger> ReadPassengersFile(string PassengerFilePath)
        {
            List<Passenger> passes = new List<Passenger>();
            try
            {
                using (StreamReader reader = new StreamReader(PassengerFilePath))
                {
                    // Skip the header row if it exists
                    string? headerLine = reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        string? line = reader.ReadLine();
                        string[] values = line!.Split(',');
                        values[1] = values[1].Replace("$", "");
                        passes.Add(new Passenger(values[0], values[1]));

                    }
                }
                Passenger.passengers = passes;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("An error occurred: " + ex.Message);
            }
            return passes;

        }

        //==== Method to read bookings file
        public static List<Booking> ReadBookingsFile(string BookingsFilePath)
        {
            List<Booking> bookings = new List<Booking>();
            try
            {
                using (StreamReader reader = new StreamReader(BookingsFilePath))
                {
                    // Skip the header row if it exists
                    string? headerLine = reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string? dataLine = reader.ReadLine();

                        string[] values = dataLine!.Split(',');
                        Enum.TryParse<FlightClass>(values[4], out FlightClass flightClass);
                        Booking booking = new Booking(Convert.ToInt32(values[1]), Convert.ToInt32(values[2]), flightClass);
                        booking.BookingId = Convert.ToInt32(values[0]);
                        bookings.Add(booking);

                    }
                }
                Booking.Bookings = bookings;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("An error occurred: " + ex.Message);
            }
            return bookings;
        }

        //==== Method to read flights file
        public static List<Flight> ReadFlightsFile(string FlightsFilePath)
        {
            List<Flight> flights = new List<Flight>();
            try
            {
                using (StreamReader reader = new StreamReader(FlightsFilePath))
                {
                    // Skip the header row if it exists
                    string? headerLine = reader.ReadLine();
                    string? temp;
                    while (!reader.EndOfStream)
                    {
                        string? dataLine = reader.ReadLine();

                        string[] values = dataLine!.Split(',');
                        decimal x;
                        bool isNumber = decimal.TryParse(values[0].Split('$')[0], out x);
                        DateTime t;
                        DateTime.TryParse(values[3], out t);
                        values[3] = t.ToString("MM/dd/yyyy hh:mm");
                        FlightService.AddFlight(new Flight(x + 1000, x + 500, x, values[1], values[2], values[3], values[4], values[5]));
                        flights.Add(new Flight(x + 1000, x + 500, x, values[1], values[2], values[3], values[4], values[5]));
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return flights;
        }
        //==== Helper function to write a CSV row
        public static void WriteCsvRow(StreamWriter writer, string[] data)
        {
            string csvLine = string.Join(",", data);
            writer.WriteLine(csvLine);
        }
    }
}
