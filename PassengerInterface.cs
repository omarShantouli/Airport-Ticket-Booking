namespace Airport_Ticket_Booking
{
    public static class PassengerInterface
    {

        //==== Method that implements Passenger Interface
        public static void Interface()
        {
            Console.Clear();
            Console.WriteLine("Airport Ticket Booking System\n" +
                              "Passenger Interface\n" +
                              "1- List Flights\n" +
                              "2- List Passengers\n" +
                              "3- Search a flight\n" +
                              "4- choose a passenger\n" +
                              "5- back");

            string? choice2;
            while (true)
            {
                choice2 = Console.ReadLine();
                if (choice2 != "1" && choice2 != "2" && choice2 != "3" && choice2 != "4" && choice2 != "5")
                {
                    Console.WriteLine("enter a valid choice!");
                }
                else if (choice2 == "1") // List Flights
                {
                    FlightService.GetFlights();
                }
                else if (choice2 == "2") // List Passengers
                {
                    Passenger.GetPassengers();
                }
                else if (choice2 == "3") // Search a flight
                {
                    Passenger.SearchFlight();
                } // end
                else if (choice2 == "4") // Choose a passenger
                {
                    Passenger.GetPassengers();
                    Console.WriteLine("Choose a passenger by its ID:");
                    string? input;
                    int passengerId;
                    while (true)
                    {
                        input = Console.ReadLine();
                        if (int.TryParse(input, out passengerId))
                        {
                            PassengerList(passengerId);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Enter a valid integer please!");
                        }
                    }
                } // end
                else
                {
                    Console.Clear();
                    break;
                }
            }
        }

        //==== Method that implements Passenger List
        public static void PassengerList(int passengerId)
        {
            Passenger? passenger = (from p in Passenger.passengers where p.PassengerId == passengerId select p).FirstOrDefault();
            if (passenger == null)
            {
                Console.WriteLine("There is no passenger with that ID!");
                return;
            }
            Console.Clear();


            string? choice;
            bool booked = false;
            bool canceled = false;
            bool modified = false;
            while (true)
            {
                Console.WriteLine($"Name: {passenger.PassengerName},ID: {passenger.PassengerId}, Phone no.: {passenger.PhoneNumber}\n");
                Console.WriteLine("1- List Flights\n" +
                                  "2- List Bookings\n" +
                                  "3- Book a Flight\n" +
                                  "4- Cancel a Flight\n" +
                                  "5- Modify a Flight\n" +
                                  "6- Search a Flight\n" +
                                  "7- Exit");
                if (booked)
                {
                    Console.WriteLine("Flight Booked successfuly!");
                    booked = false;
                }
                if (canceled)
                {
                    Console.WriteLine("Flight Canceled Seccessfuly!");
                    canceled = false;
                }
                if (modified)
                {
                    Console.WriteLine("The Book is Modified Seccessfuly!");
                    modified = false;
                }
                choice = Console.ReadLine();
                if (choice != "1" && choice != "2" && choice != "3" && choice != "4" && choice != "5" && choice != "6" && choice != "7")
                {
                    Console.WriteLine("enter a valid choice!");
                }
                else if (choice == "1") // List Flights
                {
                    FlightService.GetFlights();
                }// end
                else if (choice == "2") // List Bookings
                {
                    passenger.GetBookings();
                } // end
                else if (choice == "3") // Book a Flight
                {
                    Choices.BookFlightChoice(passenger, out booked);
                } // end
                else if (choice == "4") // Cancel a Flight
                {
                    Choices.CancelFlightChoice(passenger, out canceled);
                } // end
                else if (choice == "5") // Modify a booked flight
                {
                    Choices.ModifyFlightChoice(passenger, out modified);
                } // end
                else if (choice == "6") // Search a flight
                {
                    Passenger.SearchFlight();
                } // end
                else
                {
                    Console.Clear();
                    Console.WriteLine("Airport Ticket Booking System\n" +
                                      "1- List Flights\n" +
                                      "2- List Passengers\n" +
                                      "3- Search a Flight\n" +
                                      "4- choose a passenger\n" +
                                      "5- back");
                    return;
                }
            }
        }
    }
}
