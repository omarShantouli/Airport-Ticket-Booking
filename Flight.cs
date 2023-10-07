namespace Airport_Ticket_Booking
{
    public enum FlightClass { FirstClass, Economy, Business };
    public class Flight
    {
        public List<Passenger> Passengers = new List<Passenger>();

        public static int LastFlightNumber = 0;

        public int RemainingEconomySeats = 3;

        public int RemainingBusinessSeats = 3;

        public int RemainingFirstClassSeats = 3;

        public int RemainingSeats = 9;
        public int FlightNumber { get; init; }
        public decimal FirstClassPrice { get; init; }
        public decimal BusinessPrice { get; init; }
        public decimal EconomyPrice { get; init; }
        public string DepartureCountry { get; init; }
        public string DestinationCountry { get; init; }
        public string DepartureDate { get; init; }
        public string DepartureAirport { get; init; }
        public string ArrivalAirport { get; init; }


        public Flight(decimal firstClassPrice, decimal businessPrice, decimal economyPrice, string departureCountry, string destinationCountry, string departureDate, string departureAirport, string arrivalAirport)
        {
            FlightNumber = ++LastFlightNumber;
            FirstClassPrice = firstClassPrice;
            BusinessPrice = businessPrice;
            EconomyPrice = economyPrice;
            DepartureCountry = departureCountry;
            DestinationCountry = destinationCountry;
            DepartureDate = departureDate;
            DepartureAirport = departureAirport;
            ArrivalAirport = arrivalAirport;
        }

        //==== Method to add a passenger to the flight
        public bool AddPassenger(Passenger passenger, FlightClass @class)
        {
            try
            {
                Passengers.Add(passenger);
                if (@class == FlightClass.FirstClass)
                {
                    RemainingFirstClassSeats--;
                }
                else if (@class == FlightClass.Business)
                {
                    RemainingBusinessSeats--;
                }
                else
                {
                    RemainingEconomySeats--;
                }
                RemainingSeats--;
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        //==== Cancel a passenger from the flight
        public bool CancelPassenger(FlightClass @class)
        {
            try
            {
                if (@class == FlightClass.FirstClass)
                {
                    RemainingFirstClassSeats++;
                }
                else if (@class == FlightClass.Business)
                {
                    RemainingBusinessSeats++;
                }
                else
                {
                    RemainingEconomySeats++;
                }
                RemainingSeats++;
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
