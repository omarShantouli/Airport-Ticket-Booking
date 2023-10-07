namespace Airport_Ticket_Booking
{
    public static class ManagerInterface
    {
        public static void Interface()
        {
            string? choice;
            Console.WriteLine("Airport Ticket Booking System\n" +
                               "1- Import flights from csv file\n" +
                               "2- Search a flight\n" +
                               "3- List Flights\n" +
                               "4- List Passengers\n" +
                               "5- Exit");
            while (true)
            {
                choice = Console.ReadLine();
                if (choice != "1" && choice != "2" && choice != "3" && choice != "4" && choice != "5")
                {
                    Console.WriteLine("enter a valid choice!");
                }
                else if (choice == "1") // Import Flights
                {
                    Console.WriteLine("Enter a valid path to import flights information:");
                    string? path = Console.ReadLine();
                    path ??= "";
                    Manager.ImportFlights(path);
                }// end
                else if (choice == "2") // Search a flight
                {
                    Manager.SearchFlightManagerSide();
                }// end
                else if (choice == "3") // List flights
                {
                    FlightService.GetFlights();
                }// end
                else if (choice == "4")// List passengers
                {
                    Passenger.GetPassengers();
                }// end
                else
                {
                    Console.Clear();
                    break;
                }
            }
            return;
        }
    }
}
