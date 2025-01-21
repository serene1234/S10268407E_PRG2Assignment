//==========================================================
// Student Number	: S10268407E
// Student Name	: Serene Ker Xin Yun
// Partner Name	: Shawntrice Yip Yin Fei
//==========================================================
using S10268407E_PRG2Assignment;
using System.Collections.Generic;
using System.Globalization;

//Feature 1
//create airline and boarding gate dictionary
Dictionary<string, Airline> airlineDict = new Dictionary<string, Airline>();
Dictionary<string, BoardingGate> boardingGateDict = new Dictionary<string, BoardingGate>();

//method to load data from airlines.csv and add objects to dictionary
void CreateAirlines(Dictionary<string, Airline> airlineDict)
{
    using (StreamReader sr = new StreamReader("airlines.csv"))
    {
        // Read the header line
        string? s = sr.ReadLine();
        // Read the data lines
        while ((s = sr.ReadLine()) != null)
        {
            string[] data = s.Split(',');
            string name = data[0];
            string code = data[1];
            airlineDict.Add(code, new Airline(name, code));
        }
    }
}
//method to load data from boardinggates.csv and add objects to dictionary
void CreateBoardingGates(Dictionary<string, BoardingGate> bGateDict)
{
    using (StreamReader sr = new StreamReader("boardinggates.csv"))
    {
        // Read the header line
        string? s = sr.ReadLine();
        // Read the data lines
        while ((s = sr.ReadLine()) != null)
        {
            string[] data = s.Split(',');
            string gateName = data[0];
            bool supportsDDJB = Convert.ToBoolean(data[1]);
            bool supportsCFFT= Convert.ToBoolean(data[2]);
            bool supportsLWTT = Convert.ToBoolean(data[3]);
            bGateDict.Add(gateName, new BoardingGate(gateName, supportsDDJB, supportsCFFT, supportsLWTT));
        }
    }
}

//Feature 2
void CreateFlights(Dictionary<string, Flight> flightDict)
{
    using (StreamReader sr = new StreamReader("flights.csv"))
    {
        //read the header line
        string? s = sr.ReadLine();
        //read the data lines
        while ((s = sr.ReadLine()) != null)
        {
            string[] data = s.Split(',');
            string flightNum = data[0];
            string origin = data[1];
            string destination = data[2];
            DateTime expectedTime = DateTime.Parse(data[3]);
            string specialCode = data.Length > 4 ? data[4] : "";

            //create flight based on special code
            Flight flight;
            switch (specialCode.ToUpper())
            {
                case "DDJB":
                    flight = new DDJBFlight(flightNum, origin, destination, expectedTime);
                    break;
                case "CFFT":
                    flight = new CFFTFlight(flightNum, origin, destination, expectedTime);
                    break;
                case "LWTT":
                    flight = new LWTTFlight(flightNum, origin, destination, expectedTime);
                    break;
                default:
                    flight = new NORMFlight(flightNum, origin, destination, expectedTime);
                    break;
            }

            //add flight to dictionary
            flightDict.Add(flightNum, flight);

            //add flight to corresponding airline
            string airlineCode = flightNum.Split(' ')[0];
            if (airlineDict.ContainsKey(airlineCode))
            {
                airlineDict[airlineCode].AddFlight(flight);
            }
        }
    }
}

//Feature 3
void ListAllFlights()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-12} {1,-20} {2,-20} {3,-20} {4}",
                    "Flight No", "Origin", "Destination", "Expected Time", "Status");

    foreach (var flight in flightDict.Values)
    {
        string airlineCode = flight.FlightNumber.Split(' ')[0];
        string airlineName = airlineDict[airlineCode].Name;
        Console.WriteLine($"{flight.FlightNumber,-12} {airlineName,-20} {flight.Origin,-20} " +
                        $"{flight.Destination,-20} {flight.ExpectedTime:dd/M/yyyy h:mm:ss tt}");
    }
}

//Feature 4
//method to display all boarding gates
void DisplayAllBoardingGates(Dictionary<string, BoardingGate> bGateDict)
{
    Console.WriteLine("=============================================\r\nList of Boarding Gates for Changi Airport Terminal 5\r\n=============================================");
    Console.WriteLine($"{"Gate Name",-16}{"DDJB",-23}{"CFFT",-23}{"LWTT",-23}");
    foreach (var bGate in bGateDict.Values)
    {
        Console.WriteLine($"{bGate.GateName,-16}{bGate.SupportsDDJB,-23}{bGate.SupportsCFFT,-23}{bGate.SupportsLWTT,-23}");
    }
}

//Feature 5
void DisplayAirlineFlights()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");

    foreach (var airline in airlineDict.Values)
    {
        Console.WriteLine($"{airline.Code} {airline.Name}");
    }

    Console.Write("\nEnter Airline Code: ");
    string code = Console.ReadLine().ToUpper();

    if (airlineDict.ContainsKey(code))
    {
        var airline = airlineDict[code];
        Console.WriteLine("\nList of Flights for {0}", airline.Name);
        foreach (var flight in airline.GetFlights())
        {
            Console.WriteLine(flight.ToString());
        }
    }
    else
    {
        Console.WriteLine("Invalid airline code!");
    }
}

//Feature 7
//method to display all airlines available
void DisplayAllAirlines(Dictionary<string, Airline> airlineDict)
{
    Console.WriteLine("=============================================\r\nList of Airlines for Changi Airport Terminal 5\r\n=============================================");
    Console.WriteLine($"{"Airline Code",-16}{"Airline Name",-18}");
    foreach (var airline in airlineDict.Values)
    {
        Console.WriteLine($"{airline.Name,-16}{airline.Code,-18}");
    }
}
//method to display list of flights for a specific airline and returns airline object
Airline DisplayAirlineFlights(string code)
{
    Airline airline = airlineDict[code];
    Console.WriteLine($"=============================================\r\nList of Flights for {airline.Name}\r\n=============================================");
    Console.WriteLine($"{"Airline Number",-16}{"Origin",-18}{"Destination",-18}");
    foreach (var flight in airline.Flights.Values)
    {
        Console.WriteLine($"{flight.FlightNumber,-16}{flight.Origin,-18}{flight.Destination,-18}");
    }
    return airline;
}
//method to display flight details
void DisplayFlightDetails(string flightNo, Airline airline)
{
    Console.WriteLine("Enter Flight Number:");
    string flightNumber = Console.ReadLine();
    Console.WriteLine($"=============================================\r\nFlight Details for {flightNumber}\r\n=============================================");
    if (flightDict.ContainsKey(flightNumber))
    {
        Flight flight = flightDict[flightNumber];
        string? specialRequest = "";
        string? boardingGate = "";
        if (flight.SpecialRequestCode != null)
        {
            specialRequest = flight.SpecialRequestCode;
        }
        if (flight.BoardingGate != null)
        {
            boardingGate = flight.BoardingGate.GateName;
        }
        Console.WriteLine($"{"Flight Number",-16}{"Airline Name",-24}{"Origin",-20}{"Destination",-20}{"Expected Departure/Arrival Time",-32}{"Special Request Code",-20}{"Boarding Gate",-13}");
        Console.WriteLine($"{flight.FlightNumber,-16}{airline.Name,-24}{flight.Origin,-20}{flight.Destination,-20}{flight.ExpectedTime,-32}{specialRequest,-20}{boardingGate,-13}");
    }
    else
    {
        Console.WriteLine("Invalid Flight Number!");
    }
}

//Feature 8
//method to modify flight details
void ModifyFlightDetails(Airline airline, Flight flight)
{
    Console.WriteLine("1. Modify Basic Information");
    Console.WriteLine("2. Modify Status");
    Console.WriteLine("3. Modify Special Request Code");
    Console.WriteLine("4. Modify Boarding Gate");
    Console.WriteLine("Choose an option:");
    string? option2 = Console.ReadLine();
    if (option2  == "1")
    {
        Console.Write("Enter new Origin: ");
        string? origin = Console.ReadLine();
        Console.Write("Enter new Destination: ");
        string? destination = Console.ReadLine();
        Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
        DateTime expectedTime = Convert.ToDateTime(Console.ReadLine());
        flight.Origin = origin;
        flight.Destination = destination;
        flight.ExpectedTime = expectedTime;

    }
    else if (option2 == "2")
    {
        Console.Write("Enter new Status: ");
        string? status = Console.ReadLine();
        flight.Status = status;
    }
    else if (option2 == "3")
    {
        Console.Write("Enter new Special Request Code: ");
        string? specialRequestCode = Console.ReadLine();
        flight.SpecialRequestCode = specialRequestCode;
    }
    else if (option2 == "4")
    {
        Console.Write("Enter new Boarding Gate: ");
        string? gateName = Console.ReadLine();
        if (boardingGateDict.ContainsKey(gateName))
        {
            flight.BoardingGate = boardingGateDict[gateName];
        }
        else
        {
            Console.WriteLine("Invalid Boarding Gate!");
        }
    }
    else
    {
        Console.WriteLine("Invalid Option!");
    }
}

//method to choose an existing flight to delete
void DeleteFlight(Airline airline, Flight flight)
{
    Console.WriteLine($"Are you sure you want to delete Flight {flight.FlightNumber}? (Y/N): ");
    string? confirmation = Console.ReadLine();

    if (confirmation == "Y")
    {
        foreach (var bGate in boardingGateDict.Values)
        {
            if (bGate.Flight == flight)
            {
                bGate.Flight = null;
            }
        }
        airline.RemoveFlight(flight);
        Console.WriteLine($"Flight {flight.FlightNumber} has been deleted.");
    }
    else
    {
        Console.WriteLine("Deletion cancelled.");
    }
}

//method to display menu
void DisplayMenu()
{
    Console.WriteLine("\n\n\n\n\n\n\n\n");
    Console.WriteLine("=============================================");
    Console.WriteLine("Welcome to Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("1. List All Flights");
    Console.WriteLine("2. List Boarding Gates");
    Console.WriteLine("3. Assign a Boarding Gate to a Flight");
    Console.WriteLine("4. Create Flight");
    Console.WriteLine("5. Display Airline Flights");
    Console.WriteLine("6. Modify Flight Detail");
    Console.WriteLine("7. Display Flight Schedule");
    Console.WriteLine("0. Exit");
    Console.WriteLine("");
}

//main
CreateAirlines(airlineDict);
CreateBoardingGates(boardingGateDict);
Console.WriteLine("Loading Airlines...");
Console.WriteLine($"{airlineDict.Count} Airlines Loaded!");
Console.WriteLine($"Loading Boarding Gates...");
Console.WriteLine($"{boardingGateDict.Count} Boarding Gates Loaded!");
Console.WriteLine($"Loading Flights...");
Console.WriteLine($"{flightDict.Count} Flights Loaded!");

while (true)
{
    DisplayMenu();
    Console.WriteLine("Please select your option:");
    string? option = Console.ReadLine();
    if (option == "0")
    {
        Console.WriteLine("Goodbye!");
        break;
    }
    else if (option == "1")
    {

    }
    else if (option == "2")
    {
        DisplayAllBoardingGates(boardingGateDict);
    }
    else if (option == "3")
    {

    }
    else if (option == "4")
    {

    }
    else if (option == "5")
    {
        DisplayAllAirlines(airlineDict);
        Console.WriteLine("Enter Airline Code: ");
        string? code = Console.ReadLine();
        if (airlineDict.ContainsKey(code))
        {
            Airline airline = DisplayAirlineFlights(code);
            Console.WriteLine("Enter Flight Number: ");
            string? flightNo = Console.ReadLine();
            if (airline.Flights.ContainsKey(flightNo))
            {
                DisplayFlightDetails(flightNo, airline);
            }
            else
            {
                Console.WriteLine("Invalid Flight Number!");
                return;
            }
        }
        else
        {
            Console.WriteLine("Invalid Airline Code!");
            return;
        }
    }
    else if (option == "6")
    {
        DisplayAllAirlines(airlineDict);
        Console.WriteLine("Enter Airline Code: ");
        string? code = Console.ReadLine();
        if (airlineDict.ContainsKey(code))
        {
            Airline airline = DisplayAirlineFlights(code);
            Console.WriteLine("Choose an existing Flight to modify or delete:");
            string? flightNo = Console.ReadLine();
            if (airline.Flights.ContainsKey(flightNo))
            {
                Flight flight = airline.Flights[flightNo];
                Console.WriteLine("1. Modify Flight");
                Console.WriteLine("2. Delete Flight");
                Console.WriteLine("Choose an option:");
                string? option1 = Console.ReadLine();
                if (option1 == "1")
                {
                    ModifyFlightDetails(airline, flight);
                }
                else if (option1 == "2")
                {
                    DeleteFlight(airline, flight);
                }
                else
                {
                    Console.WriteLine("Invalid option! Please try again.");
                }
                Console.WriteLine("Flight updated!");
                Console.WriteLine("Flight Number: " + flight.FlightNumber + "\nAirline Name: " + airline.Name + "\nOrigin: " + flight.Origin + "\nDestination: " + flight.Destination + "\nExpected Departure/Arrival Time: " + flight.ExpectedTime + "\nStatus: " + flight.Status + "\nSpecial Request Code: " + flight.SpecialRequestCode + "\nBoarding Gate: " + flight.BoardingGate);
            }
            else
            {
                Console.WriteLine("Invalid Flight Number!");
                return;
            }
        }
        else
        {
            Console.WriteLine("Invalid Airline Code!");
            return;
        }
    }
    else if (option == "7")
    {

    }
    else
    {
        Console.WriteLine("Invalid option! Please try again.");
    }
}
