using Airport_Ticket_Booking;

namespace AirportTicketBooking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FileSystem.ReadFlightsFile("C:\\Users\\hanto\\Desktop\\Flights.csv");
            FileSystem.ReadPassengersFile("C:\\Users\\hanto\\Desktop\\passengers.csv");
            FileSystem.ReadBookingsFile("C:\\Users\\hanto\\Desktop\\bookings.csv");
            
            string? choice1;

            while (true)
            {
                Console.WriteLine("Airport Ticket Booking System\n" +
                                              "1- Passenger Interface\n" +
                                              "2- Manager Interface\n" +
                                              "3- exit");
                choice1 = Console.ReadLine();
                if (choice1 != "1" && choice1 != "2" && choice1 != "3")
                {
                    Console.WriteLine("enter a valid choice!");
                }
                else if (choice1 == "1") // passenger interface
                {
                    PassengerInterface.Interface();
                } // end 

                else if (choice1 == "2") // manager interface
                {
                    ManagerInterface.Interface();
                }// end

                else
                {
                    break;
                }// end 
            }
        }
    }
}