using System.Globalization;

namespace Airport_Ticket_Booking
{
    public static class Choices
    {
        //==== Method to implement booking a flight
        public static bool BookFlightChoice(Passenger passenger, out bool booked)
        {
            try
            {
                string? input;
                int flightId;
                booked = false;
                FlightService.GetFlights();
                if (FlightService.Flights.Count == 0)
                    return true;
                Console.WriteLine("Enter the ID of the flight to book:");
                while (true)
                {
                    input = Console.ReadLine();
                    if (int.TryParse(input, out flightId))
                    {
                        bool isExist = (from f in FlightService.Flights select f).Any(f => f.FlightNumber == flightId);
                        if (isExist)
                        {
                            Flight flight = (from f in FlightService.Flights where f.FlightNumber == flightId select f).Single();
                            if (flight.RemainingSeats == 0)
                            {
                                Console.WriteLine("Sorry. There is no any remaining seat on this flight.");
                                continue;
                            }

                            Booking? Conflict = (from b in Booking.Bookings
                                                 where b.DepartureDate == flight.DepartureDate
                                                        && b.PassengerId == passenger.PassengerId
                                                 select b).FirstOrDefault();

                            if (Conflict is not null)
                            {
                                Console.WriteLine($"There is a conflict with flight number : {Conflict.FlightNumber}");
                                break;
                            }

                            Console.WriteLine("Enter the class of the seat:");
                            while (true)
                            {
                                if (booked) break;
                                input = Console.ReadLine();
                                TextInfo text = new CultureInfo("en-US").TextInfo;
                                input = text.ToTitleCase(input ?? "");
                                input = input.Replace(" ", "");

                                if (Enum.TryParse<FlightClass>(input, out FlightClass flightClass))
                                {

                                    if (flightClass == FlightClass.FirstClass)
                                    {
                                        if (flight.RemainingFirstClassSeats > 0)
                                        {
                                            passenger.BookFlight(flightId, flightClass);
                                            Console.Clear();
                                            booked = true;
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Sorry. There is no remaining First Class Seat on this flight.");
                                        }
                                    }
                                    else if (flightClass == FlightClass.Business)
                                    {
                                        if (flight.RemainingBusinessSeats > 0)
                                        {
                                            passenger.BookFlight(flightId, flightClass);
                                            Console.Clear();
                                            booked = true;
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Sorry. There is no remaining First Class Seat on this flight.");
                                        }
                                    }
                                    else if (flightClass == FlightClass.Economy)
                                    {
                                        if (flight.RemainingEconomySeats > 0)
                                        {
                                            passenger.BookFlight(flightId, flightClass);
                                            Console.Clear();
                                            booked = true;
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Sorry. There is no remaining First Class Seat on this flight.");
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid Class!");
                                }
                            }
                            if (booked)
                                break;
                        }
                        else
                        {
                            Console.WriteLine("There is no flight with that ID!!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid integer.");
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                booked = false;
                return false;
            }
         }

        //==== Method to implement canceling a booked flight
        public static bool CancelFlightChoice(Passenger passenger, out bool canceled)
        {
            try
            {
                passenger.GetBookings();
                canceled = false;
                Console.WriteLine("Enter the Book ID to cancel:");
                string? input = Console.ReadLine();
                if (int.TryParse(input, out int bookingId))
                {
                    Booking? book = (from b in Booking.Bookings
                                     where b.PassengerId == passenger.PassengerId &&
                                           b.BookingId == bookingId
                                     select b).FirstOrDefault();
                    if (book is not null)
                    {
                        passenger.CancelFlight(book.FlightNumber);
                        canceled = true;
                    }
                    else
                        Console.WriteLine("Book ID does not exist!");
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                }
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                canceled = false;
                return false;
            }
        }

        //==== Method to implement modifying a book
        public static void ModifyFlightChoice(Passenger passenger, out bool modified)
        {
            passenger.GetBookings();
            modified = false;
            List<Booking> bookkings = (from b in Booking.Bookings
                                       where b.PassengerId == passenger.PassengerId
                                       select b).ToList();
            if (bookkings.Count() == 0)
            {
                Console.WriteLine("There are no bookings yet!");
                return;
            }
            Console.WriteLine("Enter the Book ID to Modify the class:");
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int bookingId))
            {
                Booking? book = (from b in Booking.Bookings
                                 where b.PassengerId == passenger.PassengerId &&
                                       b.BookingId == bookingId
                                 select b).FirstOrDefault();
                if (book is not null)
                {
                    Flight? flight = (from f in FlightService.Flights
                                      where f.FlightNumber == book.FlightNumber
                                      select f).SingleOrDefault();
                    Console.WriteLine("Enter the new class:");

                    while (true)
                    {
                        if (modified) break;
                        input = Console.ReadLine();
                        TextInfo text = new CultureInfo("en-US").TextInfo;
                        input = text.ToTitleCase(input ?? "");
                        input = input.Replace(" ", "");

                        if (Enum.TryParse<FlightClass>(input, out FlightClass newClass))
                        {

                            if (newClass == FlightClass.FirstClass)
                            {
                                if (flight!.RemainingFirstClassSeats > 0)
                                {
                                    book.Modify(newClass);
                                    Console.Clear();
                                    modified = true;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Sorry. There is no remaining First Class Seat on this flight.");
                                }
                            }
                            else if (newClass == FlightClass.Business)
                            {
                                if (flight!.RemainingBusinessSeats > 0)
                                {
                                    book.Modify(newClass);
                                    Console.Clear();
                                    modified = true;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Sorry. There is no remaining First Class Seat on this flight.");
                                }
                            }
                            else if (newClass == FlightClass.Economy)
                            {
                                if (flight!.RemainingEconomySeats > 0)
                                {
                                    book.Modify(newClass);
                                    Console.Clear();
                                    modified = true;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Sorry. There is no remaining First Class Seat on this flight.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid Class!");
                        }
                    }
                }
                else
                    Console.WriteLine("Book ID does not exist!");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid integer.");
            }

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
        }
    }
}
