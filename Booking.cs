namespace Airport_Ticket_Booking
{
    public class Booking
    {
        public static int LastId = 0;
        public int BookingId { get; set; }
        public int PassengerId { get; set; }
        public int FlightNumber { get; set; }
        public string DepartureDate { get; set; }
        public FlightClass Class { get; set; }
        public decimal Price { get; set; }

        public static List<Booking> Bookings = new List<Booking>();

        // ==== Constructor
        public Booking(int passengerId, int flightId, FlightClass @class)
        {
            PassengerId = passengerId;
            FlightNumber = flightId;
            Passenger p = (from pas in Passenger.passengers where pas.PassengerId == passengerId select pas).Single();
            Flight f = FlightService.Flights.Where(f => f.FlightNumber == flightId).Single();
            f.AddPassenger(p, @class);
            DepartureDate = f.DepartureDate; // for checking conflicts
            BookingId = ++LastId;
            this.Class = @class;
            if (Class == FlightClass.FirstClass) this.Price = f.FirstClassPrice;
            else if (Class == FlightClass.Business) this.Price = f.BusinessPrice;
            else this.Price = f.EconomyPrice;
        }

        //==== Method to modify the book
        public bool Modify(FlightClass @class)
        {
            try
            {
                Flight flight = (from f in FlightService.Flights where f.FlightNumber == this.FlightNumber select f).Single();
                FlightClass oldClass = this.Class;
                this.Class = @class;
                if (@class == FlightClass.FirstClass) { this.Price = flight.FirstClassPrice; flight.RemainingFirstClassSeats--; }
                if (@class == FlightClass.Business) { this.Price = flight.BusinessPrice; flight.RemainingBusinessSeats--; }
                else { this.Price = flight.EconomyPrice; flight.RemainingEconomySeats--; }
                if (oldClass == FlightClass.FirstClass) { flight.RemainingFirstClassSeats++; }
                else if (oldClass == FlightClass.Business) { flight.RemainingBusinessSeats++; }
                else { flight.RemainingEconomySeats++; }
                return true;
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex);
                return false;
            }

        }

        // ==== Method to display all bookings in the system
        public static bool GetBookings()
        {
            try
            {
                Booking.Bookings = (from b in Booking.Bookings orderby b.FlightNumber select b).ToList();
                foreach (Booking booking in Bookings)
                {
                    Console.WriteLine($"Passenger Id: {booking.PassengerId} | Flight Number: {booking.FlightNumber}");
                }
                return true;
            }
            catch(Exception ex) { Console.WriteLine(); return false; }
        }

    }
}
