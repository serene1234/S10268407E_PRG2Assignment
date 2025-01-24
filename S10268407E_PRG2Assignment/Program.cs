﻿//==========================================================
// Student Number	: S10268407E
// Student Name	: Serene Ker Xin Yun
// Partner Name	: Shawntrice Yip Yin Fei
//==========================================================
using S10268407E_PRG2Assignment;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Globalization;

//Feature 1
//create airline and boarding gate dictionary
Dictionary<string, Airline> airlineDict = new Dictionary<string, Airline>();
Dictionary<string, BoardingGate> boardingGateDict = new Dictionary<string, BoardingGate>();
Dictionary<string, Flight> flightDict = new Dictionary<string, Flight>();

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
    try
    {
        int count = 0;
        Console.WriteLine("Loading Flights...");
        using (StreamReader sr = new StreamReader("flights.csv"))
        {
            string? s = sr.ReadLine(); //skip header
            while ((s = sr.ReadLine()) != null)
            {
                string[] data = s.Split(',');
                if (data.Length >= 4)
                {
                    string flightNum = data[0].Trim();
                    string origin = data[1].Trim();
                    string dest = data[2].Trim();
                    DateTime expectedTime = DateTime.Parse(data[3].Trim());
                    string specialCode = data.Length > 4 ? data[4].Trim() : "";

                    Flight flight;
                    //create appropriate flight type based on special code
                    switch (specialCode.ToUpper())
                    {
                        case "DDJB":
                            flight = new DDJBFlight(flightNum, origin, dest, expectedTime);
                            break;
                        case "CFFT":
                            flight = new CFFTFlight(flightNum, origin, dest, expectedTime);
                            break;
                        case "LWTT":
                            flight = new LWTTFlight(flightNum, origin, dest, expectedTime);
                            break;
                        default:
                            flight = new NORMFlight(flightNum, origin, dest, expectedTime);
                            break;
                    }

                    //add to flight dictionary if not already exists
                    if (!flightDict.ContainsKey(flightNum))
                    {
                        flightDict.Add(flightNum, flight);
                        string airlineCode = flightNum.Split(' ')[0];
                        if (airlineDict.ContainsKey(airlineCode))
                        {
                            airlineDict[airlineCode].AddFlight(flight);
                            count++;
                        }
                    }
                }
            }
        }
        Console.WriteLine($"{count} Flights Loaded!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading flights: {ex.Message}");
    }
}

//Feature 3
void ListAllFlights()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Flight No",-12} {"Airline Name",-20} {"Origin",-20} {"Destination",-20} " +
                   $"{"Expected Time",-25} {"Status",-15}");

    //sort flights by expected time
    var sortedFlights = flightDict.Values.OrderBy(f => f.ExpectedTime).ToList();

    foreach (var flight in sortedFlights)
    {
        string airlineCode = flight.FlightNumber.Split(' ')[0];
        string airlineName = airlineDict.ContainsKey(airlineCode) ? airlineDict[airlineCode].Name : "Unknown";

        Console.WriteLine($"{flight.FlightNumber,-12} {airlineName,-20} {flight.Origin,-20} " +
                       $"{flight.Destination,-20} {flight.ExpectedTime:dd/M/yyyy h:mm:ss tt,-25} " +
                       $"{flight.Status,-15}");
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
void AssignBoardingGate()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Assign a Boarding Gate to a Flight");
    Console.WriteLine("=============================================");

    Console.WriteLine("Enter Flight Number:");
    string? flightNo = Console.ReadLine();

    if (string.IsNullOrEmpty(flightNo) || !flightDict.ContainsKey(flightNo))
    {
        Console.WriteLine("Invalid Flight Number!");
        return;
    }

    Flight flight = flightDict[flightNo];
    string airlineCode = flight.FlightNumber.Split(' ')[0];
    string airlineName = airlineDict.ContainsKey(airlineCode) ? airlineDict[airlineCode].Name : "Unknown";

    //display flight details
    Console.WriteLine($"Flight Number: {flight.FlightNumber}");
    Console.WriteLine($"Origin: {flight.Origin}");
    Console.WriteLine($"Destination: {flight.Destination}");
    Console.WriteLine($"Expected Time: {flight.ExpectedTime:dd/M/yyyy h:mm:ss tt}");

    Console.WriteLine("\nEnter Boarding Gate Name:");
    string? gateName = Console.ReadLine();

    if (string.IsNullOrEmpty(gateName) || !boardingGateDict.ContainsKey(gateName))
    {
        Console.WriteLine("Invalid Boarding Gate!");
        return;
    }

    BoardingGate gate = boardingGateDict[gateName];

    //check if gate is already assigned
    if (flightToGateDict.Values.Contains(gate))
    {
        Console.WriteLine("Gate is already assigned to another flight!");
        return;
    }

    //display gate details
    Console.WriteLine($"Boarding Gate Name: {gate.GateName}");
    Console.WriteLine($"Supports DDJB: {gate.SupportsDDJB}");
    Console.WriteLine($"Supports CFFT: {gate.SupportsCFFT}");
    Console.WriteLine($"Supports LWTT: {gate.SupportsLWTT}");

    Console.WriteLine("\nWould you like to update the status of the flight? (Y/N)");
    string? response = Console.ReadLine()?.ToUpper();

    if (response == "Y")
    {
        Console.WriteLine("1. Delayed");
        Console.WriteLine("2. Boarding");
        Console.WriteLine("3. On Time");
        Console.WriteLine("Please select the new status of the flight:");

        if (int.TryParse(Console.ReadLine(), out int statusChoice))
        {
            switch (statusChoice)
            {
                case 1:
                    flight.Status = "Delayed";
                    break;
                case 2:
                    flight.Status = "Boarding";
                    break;
                case 3:
                    flight.Status = "On Time";
                    break;
                default:
                    flight.Status = "On Time";
                    break;
            }
        }
    }

    //assign gate to flight
    flightToGateDict[flight.FlightNumber] = gate;
    Console.WriteLine($"\nFlight {flight.FlightNumber} has been assigned to Boarding Gate {gate.GateName}!");
}

//Feature 6
void CreateNewFlight()
{
    bool addAnother = true;
    while (addAnother)
    {
        Console.WriteLine("Enter Flight Number: ");
        string? flightNo = Console.ReadLine();

        if (string.IsNullOrEmpty(flightNo))
        {
            Console.WriteLine("Invalid Flight Number!");
            continue;
        }

        Console.WriteLine("Enter Origin: ");
        string? origin = Console.ReadLine();

        Console.WriteLine("Enter Destination: ");
        string? destination = Console.ReadLine();

        Console.WriteLine("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime expectedTime))
        {
            Console.WriteLine("Invalid date format!");
            continue;
        }

        Console.WriteLine("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
        string? specialCode = Console.ReadLine()?.ToUpper();

        Flight flight;
        switch (specialCode)
        {
            case "DDJB":
                flight = new DDJBFlight(flightNo, origin, destination, expectedTime);
                break;
            case "CFFT":
                flight = new CFFTFlight(flightNo, origin, destination, expectedTime);
                break;
            case "LWTT":
                flight = new LWTTFlight(flightNo, origin, destination, expectedTime);
                break;
            default:
                flight = new NORMFlight(flightNo, origin, destination, expectedTime);
                break;
        }

        //add to flight dictionary
        if (!flightDict.ContainsKey(flightNo))
        {
            flightDict.Add(flightNo, flight);
            string airlineCode = flightNo.Split(' ')[0];
            if (airlineDict.ContainsKey(airlineCode))
            {
                airlineDict[airlineCode].AddFlight(flight);
            }

            //append to flights.csv
            using (StreamWriter sw = new StreamWriter("flights.csv", true))
            {
                sw.WriteLine($"{flightNo},{origin},{destination},{expectedTime:dd/MM/yyyy HH:mm},{specialCode}");
            }

            Console.WriteLine($"Flight {flightNo} has been added!");
        }

        Console.WriteLine("Would you like to add another flight? (Y/N)");
        addAnother = Console.ReadLine()?.ToUpper() == "Y";
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
        Console.WriteLine($"{airline.Code,-16}{airline.Name,-18}");
    }
}
//method to display list of flights for a specific airline and returns airline object
Airline DisplayAirlineFlights(string code)
{
    if (airlineDict.ContainsKey(code)) {
        Airline airline = airlineDict[code];
        Console.WriteLine($"=============================================\r\nList of Flights for {airline.Name}\r\n=============================================");
        Console.WriteLine($"{"Airline Number",-16}{"Origin",-18}{"Destination",-18}");
        foreach (var flight in airline.Flights.Values)
        {
            Console.WriteLine($"{flight.FlightNumber,-16}{flight.Origin,-18}{flight.Destination,-18}");
        }
        return airline;
    }
    else
    {
        Console.WriteLine("Invalid Airline Code!");
        return null;
    }
}
//method to search for flight
Flight SearchFlight(Dictionary<string, Flight>flightDict, string flightNo)
{
    if (flightDict.ContainsKey(flightNo))
    {
        return flightDict[flightNo];
    }
    return null;
}
//dictionary to store flight to boarding gate assignment
Dictionary<string, BoardingGate> flightToGateDict = new Dictionary<string, BoardingGate>();
//method to assign boarding gate to flight
void AssignBoardingGate (string flightNo, BoardingGate gate)
{
    flightToGateDict[flightNo] = gate;
}
//method to display flight details
void DisplayFlightDetails(string flightNo, Dictionary<string, Flight>flightDict, Airline airline)
{
    Console.WriteLine($"=============================================\r\nFlight Details for {flightNo}\r\n=============================================");
    //call search() method to search for the flight
    Flight? foundFlight = SearchFlight(flightDict, flightNo);
    if (foundFlight != null)
    {
        string? specialRequest = "None";
        string? boardingGate = "Unassigned";
        //determine special request type
        if (foundFlight is DDJBFlight)
        {
            specialRequest = "DDJB";
        }
        else if (foundFlight is CFFTFlight)
        {
            specialRequest = "CFFT";
        }
        else if (foundFlight is LWTTFlight)
        {
            specialRequest = "LWTT";
        }
        //find assigned boarding gate
        if (flightToGateDict.ContainsKey(foundFlight.FlightNumber))
        {
            boardingGate = flightToGateDict[foundFlight.FlightNumber].GateName;
        }
        Console.WriteLine($"{"Flight Number",-16}{"Airline Name",-24}{"Origin",-20}{"Destination",-20}{"Expected Departure/Arrival Time",-32}{"Special Request Code",-20}{"Boarding Gate",-13}");
        Console.WriteLine($"{foundFlight.FlightNumber,-16}{airline.Name,-24}{foundFlight.Origin,-20}{foundFlight.Destination,-20}{foundFlight.ExpectedTime,-32}{specialRequest,-20}{boardingGate,-13}");
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
        Flight newFlight;
        if (specialRequestCode == "DDJB")
        {
            newFlight = new DDJBFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime);
        }
        else if (specialRequestCode == "CFFT")
        {
            newFlight = new CFFTFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime);
        }
        else if (specialRequestCode == "LWTT")
        {
            newFlight = new LWTTFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime);
        }
        else
        {
            newFlight = new NORMFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime);
        }
        //replace flight in airline dictionary
        airline.Flights[flight.FlightNumber] = newFlight;
        //update flight-to-gate assignment
        if (flightToGateDict.ContainsKey(flight.FlightNumber))
        {
            BoardingGate assignedGate = flightToGateDict[flight.FlightNumber];
            flightToGateDict.Remove(flight.FlightNumber);
            flightToGateDict[newFlight.FlightNumber] = flightToGateDict[flight.FlightNumber];
        }
    }
    else if (option2 == "4")
    {
        Console.Write("Enter new Boarding Gate: ");
        string? gateName = Console.ReadLine();
        if (boardingGateDict.ContainsKey(gateName))
        {
            flightToGateDict[flight.FlightNumber] = boardingGateDict[gateName];
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
CreateFlights(flightDict);
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

        Airline? airline = DisplayAirlineFlights(code);
        if (airline != null)
        {
            Console.WriteLine("Enter Flight Number: ");
            string? flightNo = Console.ReadLine();

            //search for flight in alirline's flight 
            Flight? selectedFlight = SearchFlight(flightDict, flightNo);
            if (selectedFlight != null && airline.Flights.ContainsKey(selectedFlight.FlightNumber))
            {
                DisplayFlightDetails(flightNo, flightDict, airline);
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
                    flight = airline.Flights[flightNo];
                }
                else if (option1 == "2")
                {
                    DeleteFlight(airline, flight);
                }
                else
                {
                    Console.WriteLine("Invalid option! Please try again.");
                }
                string specialRequestCode = "None";
                if (flight is DDJBFlight)
                {
                    specialRequestCode = "DDJB";
                }
                else if (flight is CFFTFlight)
                {
                    specialRequestCode = "CFFT";
                }
                else if (flight is LWTTFlight)
                {
                    specialRequestCode = "LWTT";
                }
                string boardingGate = "Unassigned"; 
                if (flightToGateDict.ContainsKey(flight.FlightNumber))
                {
                    boardingGate = flightToGateDict[flight.FlightNumber].GateName;
                }
                Console.WriteLine("Flight updated!");
                Console.WriteLine("Flight Number: " + flight.FlightNumber + "\nAirline Name: " + airline.Name + "\nOrigin: " + flight.Origin + "\nDestination: " + flight.Destination + "\nExpected Departure/Arrival Time: " + flight.ExpectedTime + "\nStatus: " + flight.Status + "\nSpecial Request Code: " + specialRequestCode +  "\nGate: " + boardingGate);
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

//Feature 9
void DisplayFlightSchedule()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Flight No",-12} {"Airline",-20} {"Origin",-20} {"Destination",-20} " +
                     $"{"Expected Time",-25} {"Status",-15} {"Gate"}");

    //sort flights by expected time
    var sortedFlights = flightDict.Values
        .OrderBy(f => f.ExpectedTime)
        .ToList();

    foreach (var flight in sortedFlights)
    {
        string airlineCode = flight.FlightNumber.Split(' ')[0];
        string airlineName = airlineDict.ContainsKey(airlineCode) ? airlineDict[airlineCode].Name : "Unknown";
        string gate = flightToGateDict.ContainsKey(flight.FlightNumber) ?
            flightToGateDict[flight.FlightNumber].GateName : "Unassigned";

        Console.WriteLine($"{flight.FlightNumber,-12} {airlineName,-20} {flight.Origin,-20} " +
                         $"{flight.Destination,-20} {flight.ExpectedTime:dd/M/yyyy h:mm:ss tt,-25} " +
                         $"{flight.Status,-15} {gate}");
    }
}

//Advanced Feature (a)
void ProcessUnassignedFlights()
{
    //create queue for unassigned flights
    Queue<Flight> unassignedFlights = new Queue<Flight>();
    int totalUnassigned = 0;
    int totalUnassignedGates = 0;

    //check for unassigned flights
    foreach (var flight in flightDict.Values)
    {
        if (!flightToGateDict.ContainsKey(flight.FlightNumber))
        {
            unassignedFlights.Enqueue(flight);
            totalUnassigned++;
        }
    }
    Console.WriteLine($"Total unassigned flights: {totalUnassigned}");

    //count available gates
    var availableGates = boardingGateDict.Values
        .Where(g => !flightToGateDict.Values.Contains(g))
        .ToList();
    totalUnassignedGates = availableGates.Count;
    Console.WriteLine($"Total available gates: {totalUnassignedGates}");

    if (totalUnassigned == 0)
    {
        Console.WriteLine("No unassigned flights to process.");
        return;
    }

    int processedCount = 0;
    int automaticAssignments = 0;

    //process each unassigned flight
    while (unassignedFlights.Count > 0 && availableGates.Count > 0)
    {
        var flight = unassignedFlights.Dequeue();
        BoardingGate assignedGate = null;

        //check flight type and find matching gate
        if (flight is DDJBFlight)
        {
            assignedGate = availableGates.FirstOrDefault(g => g.SupportsDDJB);
        }
        else if (flight is CFFTFlight)
        {
            assignedGate = availableGates.FirstOrDefault(g => g.SupportsCFFT);
        }
        else if (flight is LWTTFlight)
        {
            assignedGate = availableGates.FirstOrDefault(g => g.SupportsLWTT);
        }
        else //NORMFlight
        {
            assignedGate = availableGates.FirstOrDefault(g =>
                !g.SupportsDDJB && !g.SupportsCFFT && !g.SupportsLWTT);
        }

        //if found matching gate, assign it
        if (assignedGate != null)
        {
            flightToGateDict[flight.FlightNumber] = assignedGate;
            availableGates.Remove(assignedGate);
            automaticAssignments++;

            //display assignment details
            string airlineCode = flight.FlightNumber.Split(' ')[0];
            string airlineName = airlineDict.ContainsKey(airlineCode) ? airlineDict[airlineCode].Name : "Unknown";

            Console.WriteLine("\nAssigned Flight:");
            Console.WriteLine($"{"Flight No",-12} {"Airline",-20} {"Origin",-20} {"Destination",-20} " +
                            $"{"Expected Time",-25} {"Special Request",-15} {"Assigned Gate"}");

            string specialRequest = flight switch
            {
                DDJBFlight => "DDJB",
                CFFTFlight => "CFFT",
                LWTTFlight => "LWTT",
                _ => "None"
            };

            Console.WriteLine($"{flight.FlightNumber,-12} {airlineName,-20} {flight.Origin,-20} " +
                            $"{flight.Destination,-20} {flight.ExpectedTime:dd/M/yyyy h:mm:ss tt,-25} " +
                            $"{specialRequest,-15} {assignedGate.GateName}");
        }

        processedCount++;
    }

    //display summary
    Console.WriteLine("\nAssignment Summary:");
    Console.WriteLine($"Total flights processed: {processedCount}");
    Console.WriteLine($"Successful automatic assignments: {automaticAssignments}");

    double automaticPercentage = (double)automaticAssignments / totalUnassigned * 100;
    Console.WriteLine($"Automatic assignment percentage: {automaticPercentage:F1}%");

    if (unassignedFlights.Count > 0)
    {
        Console.WriteLine($"\nWarning: {unassignedFlights.Count} flights remain unassigned due to insufficient matching gates.");
    }
}