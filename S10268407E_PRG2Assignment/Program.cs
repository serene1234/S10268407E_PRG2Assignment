//==========================================================
// Student Number	: S10268407E
// Student Name	: Serene Ker Xin Yun
// Partner Name	: Shawntrice Yip Yin Fei
//==========================================================
using S10268407E_PRG2Assignment;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Diagnostics.Tracing;
using System.Globalization;

//Feature 1
//method to load data from airlines.csv and add objects to dictionary
void CreateAirlines(Terminal terminal)
{
    try
    {
        //read airlines.csv file
        using (StreamReader sr = new StreamReader("airlines.csv"))
        {
            //read the header line
            string? s = sr.ReadLine();
            //read the data lines
            while ((s = sr.ReadLine()) != null)
            {
                string[] data = s.Split(',');
                string name = data[0];
                string code = data[1];
                //add airline name and code to airline dictionary
                Airline airline = new Airline(name, code);
                terminal.AddAirline(airline);
            }
        }
    }
    //error handling for file operations
    catch (FileNotFoundException ex)
    {
        Console.WriteLine($"Error loading airlines: {ex.Message}");
    }
    //general exception handling
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading airlines: {ex.Message}");
    }
}
//method to load data from boardinggates.csv and add objects to dictionary
void CreateBoardingGates(Terminal terminal)
{
    try
    {
        //read boardinggates.csv file
        using (StreamReader sr = new StreamReader("boardinggates.csv"))
        {
            //read the header line
            string? s = sr.ReadLine();
            //read the data lines
            while ((s = sr.ReadLine()) != null)
            {
                string[] data = s.Split(',');
                string gateName = data[0];
                bool supportsDDJB = Convert.ToBoolean(data[1]);
                bool supportsCFFT = Convert.ToBoolean(data[2]);
                bool supportsLWTT = Convert.ToBoolean(data[3]);
                //add boarding gate details to boarding gate dictionary
                BoardingGate bgate = new BoardingGate(gateName, supportsDDJB, supportsCFFT, supportsLWTT);
                terminal.AddBoardingGate(bgate);
            }
        }
    }
    //error handling for file operations
    catch (FileNotFoundException ex)
    {
        Console.WriteLine($"Error loading boarding gates: {ex.Message}");
    }
    //general exception handling
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading boarding gates: {ex.Message}");
    }
}

//Feature 2
void CreateFlights(Terminal terminal)
{
    try
    {
        int count = 0;

        //open and read flights.csv file
        using (StreamReader sr = new StreamReader("flights.csv"))
        {
            string? s = sr.ReadLine(); //skip header row

            //process each line of the CSV file
            while ((s = sr.ReadLine()) != null)
            {
                string[] data = s.Split(',');

                //validate we have minimum required fields
                if (data.Length >= 4)
                {
                    //extract and clean flight data
                    string flightNum = data[0].Trim();
                    string origin = data[1].Trim();
                    string dest = data[2].Trim();
                    DateTime expectedTime = DateTime.Parse(data[3].Trim());

                    //check for optional special request code
                    string specialCode = data.Length > 4 ? data[4].Trim() : "";

                    Flight flight;
                    //create specific flight type based on special code
                    //DDJB = Double-decker jet bridge
                    //CFFT = Connecting flight fast transfer
                    //LWTT = Longer waiting time
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

                    //add flight to dictionary if not already exists
                    if (!terminal.Flights.ContainsKey(flightNum))
                    {
                        terminal.Flights.Add(flightNum, flight);

                        //extract airline code from flight number (e.g., "SQ" from "SQ 123")
                        string airlineCode = flightNum.Split(' ')[0];

                        //associate flight with airline if airline exists
                        if (terminal.Airlines.ContainsKey(airlineCode))
                        {
                            terminal.Airlines[airlineCode].AddFlight(flight);
                            count++;
                        }
                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        //error handling for file operations
        Console.WriteLine($"Error loading flights: {ex.Message}");
    }
}

//Feature 3
void ListAllFlights(Terminal terminal)
{
    //display header with consistent formatting
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");

    //define column headers with precise spacing for alignment
    Console.WriteLine($"{"Flight Number",-15} {"Airline Name",-20} {"Origin",-20} {"Destination",-20} " +
                   $"{"Expected Departure/Arrival Time",-25}");

    //sort all flights by expected time for chronological display
    //using LINQ OrderBy to sort the dictionary values
    var sortedFlights = terminal.Flights.Values
        .OrderBy(f => f.ExpectedTime)
        .ToList();

    //iterate through sorted flights and display their information
    foreach (var flight in sortedFlights)
    {
        //extract airline code from flight number (e.g., "SQ" from "SQ 123")
        string airlineCode = flight.FlightNumber.Split(' ')[0];

        //get airline name, defaulting to "Unknown" if code not found
        string airlineName = terminal.Airlines.ContainsKey(airlineCode)
            ? terminal.Airlines[airlineCode].Name
            : "Unknown";

        //format each line with consistent column spacing
        //using DateTime format "dd/M/yyyy h:mm:ss tt" as per requirements
        Console.WriteLine($"{flight.FlightNumber,-15} {airlineName,-20} " +
                 $"{flight.Origin,-20} {flight.Destination,-20} " +
                 $"{flight.ExpectedTime:dd/M/yyyy h:mm:ss tt}");
    }
}

//Feature 4
//method to display all boarding gates
void DisplayAllBoardingGates(Terminal terminal)
{
    Console.WriteLine("=============================================\r\nList of Boarding Gates for Changi Airport Terminal 5\r\n=============================================");
    Console.WriteLine($"{"Gate Name",-16}{"DDJB",-23}{"CFFT",-23}{"LWTT",-23}{"Flight Number", -13}");
    //iterate through boarding gate dictionary and display details
    foreach (var bGate in terminal.BoardingGates.Values)
    {
        if (bGate.Flight != null)
        {
            Console.WriteLine($"{ bGate.ToString()}{bGate.Flight.FlightNumber}");
        }
        else
        {
            Console.WriteLine($"{bGate.ToString()}{"Unassigned",-13}");
        }
    }
}

//Feature 5
void AssignBoardingGate(Terminal terminal)
{
    //display feature header
    Console.WriteLine("=============================================");
    Console.WriteLine("Assign a Boarding Gate to a Flight");
    Console.WriteLine("=============================================");

    //get flight number input
    Console.Write("Enter Flight Number: ");
    string? flightNo = Console.ReadLine();

    //check if flight number exists in dictionary
    if (string.IsNullOrEmpty(flightNo) || !terminal.Flights.ContainsKey(flightNo))
    {
        Console.WriteLine("Invalid Flight Number!");
        return;
    }

    //get flight object
    Flight flight = terminal.Flights[flightNo];

    //get airline name
    string airlineCode = flightNo.Split(' ')[0];
    string airlineName = terminal.Airlines.ContainsKey(airlineCode) ? terminal.Airlines[airlineCode].Name : "Unknown";

    //get special request and gate status
    string specialRequest = flight switch
    {
        DDJBFlight => "DDJB",
        CFFTFlight => "CFFT",
        LWTTFlight => "LWTT",
        _ => "None"
    };

    //get boarding gate input
    Console.Write("Enter Boarding Gate Name: ");
    string? gateName = Console.ReadLine();

    //validate boarding gate
    if (string.IsNullOrEmpty(gateName) || !terminal.BoardingGates.ContainsKey(gateName))
    {
        Console.WriteLine("Invalid Boarding Gate!");
        return;
    }

    BoardingGate gate = terminal.BoardingGates[gateName];

    //check if gate is already assigned
    if (gate.Flight != null)
    {
        Console.WriteLine($"Boarding Gate {gateName} is already assigned to another flight!");
        return;
    }

    //display gate details
    Console.WriteLine($"Flight Number: {flightNo}");
    Console.WriteLine($"Origin: {flight.Origin}");
    Console.WriteLine($"Destination: {flight.Destination}");
    Console.WriteLine($"Expected Time: {flight.ExpectedTime:dd/M/yyyy h:mm:ss tt}");
    Console.WriteLine($"Special Request Code: {specialRequest}");
    Console.WriteLine($"Boarding Gate Name: {gateName}");
    Console.WriteLine($"Supports DDJB: {gate.SupportsDDJB}");
    Console.WriteLine($"Supports CFFT: {gate.SupportsCFFT}");
    Console.WriteLine($"Supports LWTT: {gate.SupportsLWTT}");

    //handle status update
    Console.Write("Would you like to update the status of the flight? (Y/N) ");
    string? response = Console.ReadLine()?.ToUpper();

    if (response == "Y")
    {
        Console.WriteLine("1. Delayed");
        Console.WriteLine("2. Boarding");
        Console.WriteLine("3. On Time");
        Console.Write("Please select the new status of the flight: ");

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
            }
        }
    }
    else
    {
        flight.Status = "On Time";
    }

    //assign gate and display confirmation
    gate.Flight = flight;
    Console.WriteLine($"\nFlight {flightNo} has been assigned to Boarding Gate {gateName}!");
}

//Feature 6
void CreateNewFlight(Terminal terminal)
{
    bool addAnother = true;
    while (addAnother)
    {
        //get flight number
        Console.WriteLine("Enter Flight Number: ");
        string? flightNo = Console.ReadLine();

        if (string.IsNullOrEmpty(flightNo))
        {
            Console.WriteLine("Invalid Flight Number!");
            continue;
        }

        //get origin and destination
        Console.WriteLine("Enter Origin: ");
        string? origin = Console.ReadLine();

        Console.WriteLine("Enter Destination: ");
        string? destination = Console.ReadLine();

        //get and validate departure time
        Console.WriteLine("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime expectedTime))
        {
            Console.WriteLine("Invalid date format!");
            continue;
        }

        //handle special request codes
        Console.WriteLine("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
        string? specialCode = Console.ReadLine()?.ToUpper();

        //create appropriate flight type based on special code
        Flight flight;
        switch (specialCode)
        {
            case "DDJB": //double-decker jet bridge
                flight = new DDJBFlight(flightNo, origin, destination, expectedTime);
                break;
            case "CFFT": //connecting flight fast transfer
                flight = new CFFTFlight(flightNo, origin, destination, expectedTime);
                break;
            case "LWTT": //longer waiting time
                flight = new LWTTFlight(flightNo, origin, destination, expectedTime);
                break;
            default: //normal flight
                flight = new NORMFlight(flightNo, origin, destination, expectedTime);
                break;
        }

        //add flight to system
        if (!terminal.Flights.ContainsKey(flightNo))
        {
            //add to flight dictionary
            terminal.Flights.Add(flightNo, flight);

            //add to airline's flight list
            string airlineCode = flightNo.Split(' ')[0];
            if (terminal.Airlines.ContainsKey(airlineCode))
            {
                terminal.Airlines[airlineCode].AddFlight(flight);
            }

            //persist to CSV file
            using (StreamWriter sw = new StreamWriter("flights.csv", true))
            {
                sw.WriteLine($"{flightNo},{origin},{destination}," +
                           $"{expectedTime:dd/MM/yyyy HH:mm},{specialCode}");
            }

            Console.WriteLine($"Flight {flightNo} has been added!");
        }

        //check if user wants to add another flight
        Console.WriteLine("Would you like to add another flight? (Y/N)");
        addAnother = Console.ReadLine()?.ToUpper() == "Y";
    }
}

//Feature 7
//method to display all airlines available
void DisplayAllAirlines(Terminal terminal)
{
    Console.WriteLine("=============================================\r\nList of Airlines for Changi Airport Terminal 5\r\n=============================================");
    Console.WriteLine($"{"Airline Code",-16}{"Airline Name",-18}");
    //iterate through airline dictionary and display details
    foreach (var airline in terminal.Airlines.Values)
    {
        Console.WriteLine(airline.ToString());
    }
}
//method to display list of flights for a specific airline and returns airline object
Airline DisplayAirlineFlights(Terminal terminal, string code)
{
    //get airline object
    Airline airline = terminal.Airlines[code];
    Console.WriteLine($"=============================================\r\nList of Flights for {airline.Name}\r\n=============================================");
    Console.WriteLine($"{"Airline Number",-16}{"Origin",-18}{"Destination",-18}");
    //iterate through airline's flights and display details
    foreach (var flight in airline.Flights.Values)
    {
        Console.WriteLine($"{flight.FlightNumber,-16}{flight.Origin,-18}{flight.Destination,-18}");
    }
    //return airline object
    return airline;
}
//method to search for flight
Flight SearchFlight(Terminal terminal, string flightNo)
{
    //search for flight number in dictionary
    if (terminal.Flights.ContainsKey(flightNo))
    {
        //return flight object
        return terminal.Flights[flightNo];
    }
    else
    {
        return null;

    }
}
//method to display flight details
void DisplayFlightDetails(Terminal terminal, string flightNo)
{
    try
    {
        Console.WriteLine($"=============================================\r\nFull Flight Details for {flightNo}\r\n=============================================");
        //call search() method to search for the flight
        Flight? foundFlight = SearchFlight(terminal, flightNo);
        if (foundFlight != null)
        {
            //get airline object from flight object
            Airline airline = terminal.GetAirlineFromFlight(foundFlight);
            //initialise special request and boarding gate variables
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
            foreach (var bGate in terminal.BoardingGates.Values)
            {
                //check if flight is assigned to a boarding gate
                if (bGate.Flight != null && bGate.Flight.FlightNumber == foundFlight.FlightNumber)
                {
                    //assign boarding gate to variable
                    boardingGate = bGate.GateName;
                    break;
                }
            }
            //display flight details
            Console.WriteLine($"{"Flight Number",-16}{"Airline Name",-24}{"Origin",-20}{"Destination",-20}{"Expected Departure/Arrival Time",-34}");
            Console.WriteLine($"{foundFlight.FlightNumber,-16}{airline.Name,-24}{foundFlight.Origin,-20}{foundFlight.Destination,-20}{foundFlight.ExpectedTime,-34}");
            Console.WriteLine($"Special Request Code: {specialRequest}");
            Console.WriteLine($"Boarding Gate: {boardingGate}");
        }
        //error handling for invalid flight number
        else
        {
            Console.WriteLine("Flight Number not found!");
            return;
        }
    }
    //error handling for invalid input
    catch (FormatException ex)
    {
        Console.WriteLine(ex.Message);
    }
    //error handling for missing flight number
    catch (KeyNotFoundException ex)
    {
        Console.WriteLine(ex.Message);
    }
    //general exception handling
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
void HandleDisplayAirlineFlights (Terminal terminal)
{
    DisplayAllAirlines(terminal);
    Console.Write("Enter Airline Code: ");
    string? code = Console.ReadLine()?.ToUpper();
    //check if code is empty or not 2 characters
    if (string.IsNullOrEmpty(code) || code.Length != 2)
    {
        throw new FormatException("Invalid Airline Code! The code must be exactly 2 characters.");
    }
    //check if airline code exists
    if (!terminal.Airlines.ContainsKey(code))
    {
        throw new KeyNotFoundException("Airline Code not found!");
    }
    //display airline flights
    Airline? airline = DisplayAirlineFlights(terminal, code);

    Console.Write("Enter Flight Number: ");
    string? flightNo = Console.ReadLine()?.ToUpper();
    if (string.IsNullOrEmpty(flightNo))
    {
        throw new FormatException("Invalid Flight Number! The number cannot be empty.");
    }
    //search for flight in alirline's flight 
    Flight? selectedFlight = SearchFlight(terminal, flightNo);
    if (selectedFlight != null && airline.Flights.ContainsKey(selectedFlight.FlightNumber))
    {
        DisplayFlightDetails(terminal, flightNo);
    }
    else
    {
        throw new KeyNotFoundException("Flight Number not found in airline's flights.");
    }
}

//Feature 8
//method to update flight details
void ModifyFlightDetails(Terminal terminal, Flight flight)
{
    Console.WriteLine("1. Modify Basic Information");
    Console.WriteLine("2. Modify Status");
    Console.WriteLine("3. Modify Special Request Code");
    Console.WriteLine("4. Modify Boarding Gate");
    Console.WriteLine("Choose an option:");
    string? modifyOption = Console.ReadLine();
    if (string.IsNullOrEmpty(modifyOption))
    {
        Console.WriteLine("Invalid option! Please choose option between 1-4.");
        return;
    }
    //get airline object from flight object
    Airline airline = terminal.GetAirlineFromFlight(flight);
    //check if flight details are updated
    bool isUpdated = false;
    try
    {
        if (modifyOption == "1")
        {
            //get new origin
            Console.Write("Enter new Origin: ");
            string? origin = Console.ReadLine();
            //check if origin input is empty
            if (string.IsNullOrEmpty(origin)) throw new FormatException("Origin cannot be empty.");
            //get new destination
            Console.Write("Enter new Destination: ");
            string? destination = Console.ReadLine();
            //check if destination input is empty
            if (string.IsNullOrEmpty(destination)) throw new FormatException("Destination cannot be empty.");
            //get new expected time
            Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
            string? time = Console.ReadLine();
            //validate expected time input
            try
            {
                DateTime expectedTime = DateTime.Parse(time);
                //check if expected time is in the past
                if (expectedTime < DateTime.Now)
                {
                    throw new ArgumentException("Expected time cannot be in the past.");
                }
                //update flight details
                flight.Origin = origin;
                flight.Destination = destination;
                flight.ExpectedTime = expectedTime;
                //set flag to true
                isUpdated = true;
            }
            catch (FormatException)
            {
                throw new FormatException("Invalid date format!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        else if (modifyOption == "2")
        {
            //get new status
            Console.Write("Enter new Status: ");
            string? status = Console.ReadLine();
            //check if status input is empty
            if (string.IsNullOrEmpty(status)) throw new FormatException("Status cannot be empty.");
            //update flight status
            flight.Status = status;
            //set flag to true
            isUpdated = true;
        }
        else if (modifyOption == "3")
        {
            //get new special request code
            Console.Write("Enter new Special Request Code: ");
            string? specialRequestCode = Console.ReadLine()?.ToUpper();
            //check if special request code input is empty
            if (string.IsNullOrEmpty(specialRequestCode)) throw new FormatException("Special request code cannot be empty.");
            //check if the special request code is valid
            if (specialRequestCode != "DDJB" && specialRequestCode != "CFFT" && specialRequestCode != "LWTT" && specialRequestCode != "NORM")
            {
                throw new ArgumentException("Invalid special request code entered.");
            }
            //create new flight object based on special request code
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
            terminal.Flights.Remove(flight.FlightNumber);
            terminal.Flights.Add(newFlight.FlightNumber, newFlight);
            //set flag to true
            isUpdated = true;
        }
        else if (modifyOption == "4")
        {
            //check if flight is already assigned to a boarding gate
            BoardingGate? currentFlightGate = null;
            foreach (var bGate in terminal.BoardingGates.Values)
            {
                if (bGate.Flight == flight)
                {
                    currentFlightGate = bGate;
                    break;
                }
            }
            if (currentFlightGate == null) throw new InvalidOperationException("Flight is not assigned to any boarding gate.");
            //get new boarding gate
            Console.Write("Enter new Boarding Gate: ");
            string? gateName = Console.ReadLine()?.ToUpper();
            //check if boarding gate input is empty
            if (string.IsNullOrEmpty(gateName)) throw new FormatException("Boarding Gate cannot be empty.");
            //variable to hold current boarding gate if found  
            BoardingGate? newGate = null;
            //iterate through boarding gate dictionary to find matching gate
            foreach (var bGate in terminal.BoardingGates.Values)
            {
                //check if gate name matches user input
                if (bGate.GateName == gateName)
                {
                    //check if gate is not assigned to any flight
                    if (bGate.Flight == null)
                    {
                        //assign matching gate to currentGate variable
                        newGate = bGate;
                        break;
                    }
                    //error handling for gate already assigned to another flight
                    else
                    {
                        throw new InvalidOperationException("The gate is already assigned to another flight!");
                    }
                }
            }
            //assign flight to new gate if gate is found
            if (newGate != null)
            {
                //remove flight from current gate
                currentFlightGate.Flight = null;
                //assign flight to new gate
                newGate.Flight = flight;
                //set flag to true
                isUpdated = true;
            }
            //error handling for no valid boarding gate found
            else
            {
                throw new InvalidOperationException("Invalid boarding gate!");
            }
        }
        //error handling for invalid modify option
        else
        {
            throw new FormatException("Invalid option! Please choose option between 1-4.");
        }
        //display updated flight details if flight details are updated
        if (isUpdated)
        {
            DisplayUpdatedFlightDetails(terminal, flight.FlightNumber);
        }
    }
    //error handling for invalid input formats
    catch (FormatException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    //error handling for invalid boarding gate
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    //error handling for invalid special request code
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    //general exception handling
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected error: {ex.Message}");
    }
}
//method to display updated flight details
void DisplayUpdatedFlightDetails(Terminal terminal, string flightNo)
{
    try
    {
        //search for flight in flight dictionary
        Flight? foundFlight = SearchFlight(terminal, flightNo);
        //get airline object from flight object
        Airline airline = terminal.GetAirlineFromFlight(foundFlight);
        //display updated flight details
        if (foundFlight != null)
        {
            //initialise special request code and boarding gate variables
            string? specialRequestCode = "None";
            string? boardingGate = "Unassigned";
            //determine special request type based on flight type
            if (foundFlight is DDJBFlight)
            {
                specialRequestCode = "DDJB";
            }
            else if (foundFlight is CFFTFlight)
            {
                specialRequestCode = "CFFT";
            }
            else if (foundFlight is LWTTFlight)
            {
                specialRequestCode = "LWTT";
            }
            //iterate through boarding gate dictionary to find boarding gate assigned to the flight
            foreach (var bGate in terminal.BoardingGates.Values)
            {
                //check if flight is assigned to a boarding gate
                if (bGate.Flight != null && bGate.Flight.FlightNumber == foundFlight.FlightNumber)
                {
                    //assign boarding gate to variable
                    boardingGate = bGate.GateName;
                    break;
                }
            }
            //display updated flight details
            Console.WriteLine("Flight updated!");
            Console.WriteLine("Flight Number: " + foundFlight.FlightNumber + "\nAirline Name: " + airline.Name + "\nOrigin: " + foundFlight.Origin + "\nDestination: " + foundFlight.Destination + "\nExpected Departure/Arrival Time: " + foundFlight.ExpectedTime + "\nStatus: " + foundFlight.Status + "\nSpecial Request Code: " + specialRequestCode + "\nBoarding Gate: " + boardingGate);
        }
        //error handling for invalid flight number
        else
        {
            throw new FormatException("Invalid Flight Number!");
        }
    }
    catch (FormatException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (KeyNotFoundException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

}
//method to choose an existing flight to delete
void DeleteFlight(Terminal terminal, Flight flight)
{
    try
    {
        //get airline object from flight object
        Airline airline = terminal.GetAirlineFromFlight(flight);
        //display confirmation message
        Console.WriteLine($"Are you sure you want to delete Flight {flight.FlightNumber}? (Y/N): ");
        string? confirmation = Console.ReadLine()?.ToUpper();
        //check if confirmation input is empty
        if (string.IsNullOrEmpty(confirmation))
        {
            throw new FormatException("Invalid input! Please enter Y or N.");
        }
        //process deletion based on user input
        if (confirmation == "Y") //if user confirms deletion
        {
            //unassign flight from boarding gate if it is assigned
            foreach (var bGate in terminal.BoardingGates.Values)
            {
                //check if flight is assigned to a boarding gate
                if (bGate.Flight == flight)
                {
                    //unassign flight from boarding gate
                    bGate.Flight = null;
                    break;
                }
            }
            //remove flight from airline's list of flights
            airline.RemoveFlight(flight);
            Console.WriteLine($"Flight {flight.FlightNumber} has been deleted.");
        }
        else if (confirmation == "N")  //if user cancels deletion
        {
            Console.WriteLine("Deletion cancelled.");
        }
        //error handling for invalid confirmation input
        else
        {
            throw new FormatException("Invalid input! Please enter Y or N.");
        }
    }
    //error handling for invalid input formats
    catch (FormatException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    //general exception handling
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected error: {ex.Message}");
    }
}
void HandleModifyOrDeleteFlight(Terminal terminal)
{
    DisplayAllAirlines(terminal);
    Console.Write("Enter Airline Code: ");
    string? code = Console.ReadLine()?.ToUpper();
    //check if code is empty or not 2 characters
    if (string.IsNullOrEmpty(code) || code.Length != 2)
    {
        throw new FormatException("Invalid Airline Code! The code must be exactly 2 characters.");
    }
    if (!terminal.Airlines.ContainsKey(code))
    {
        throw new KeyNotFoundException("Airline Code not found!");
    }
    Airline airline = DisplayAirlineFlights(terminal, code);
    Console.Write("Choose an existing Flight to modify or delete: ");
    string? flightNo = Console.ReadLine()?.ToUpper();
    if (string.IsNullOrEmpty(flightNo))
    {
        throw new FormatException("Invalid Flight Number! The number cannot be empty.");
    }
    if (!airline.Flights.ContainsKey(flightNo))
    {
        throw new KeyNotFoundException("Flight Number not found in airline's flights.");
    }
    Flight flight = airline.Flights[flightNo];
    Console.WriteLine("1. Modify Flight");
    Console.WriteLine("2. Delete Flight");
    Console.Write("Choose an option: ");
    string? modifyOption = Console.ReadLine();
    if (modifyOption == "1")
    {
        ModifyFlightDetails(terminal, flight);
    }
    else if (modifyOption == "2")
    {
        DeleteFlight(terminal, flight);
    }
    else
    {
        throw new InvalidOperationException("Invalid option! Please choose option between 1-2.");
    }
}

//Feature 9
void DisplayFlightSchedule(Terminal terminal)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");

    //column headers matching sample output format exactly
    Console.WriteLine($"{"Flight Number",-15} {"Airline",-20} {"Origin",-20} {"Destination",-20} " +
                     $"{"Expected Time",-20} {"Status",-12} {"Gate"}");

    //sort flights chronologically using IComparable implementation
    var sortedFlights = terminal.Flights.Values
        .OrderBy(f => f.ExpectedTime)
        .ToList();

    foreach (var flight in sortedFlights)
    {
        string airlineCode = flight.FlightNumber.Split(' ')[0];
        string airlineName = terminal.Airlines.ContainsKey(airlineCode)
            ? terminal.Airlines[airlineCode].Name
            : "Unknown";

        //find gate assignment (if any)
        string gateAssignment = "Unassigned";
        foreach (var gate in terminal.BoardingGates.Values)
        {
            if (gate.Flight?.FlightNumber == flight.FlightNumber)
            {
                gateAssignment = gate.GateName;
                break;
            }
        }

        //display flight info in required format
        string formattedTime = flight.ExpectedTime.ToString("dd/M/yyyy h:mm:ss tt");
        Console.WriteLine($"{flight.FlightNumber,-15} {airlineName,-20} " +
                       $"{flight.Origin,-20} {flight.Destination,-20} " +
                       $"{formattedTime,-20} {flight.Status,-12} {gateAssignment}");
    }
}

//Advanced Feature (a)
void ProcessUnassignedFlights(Terminal terminal)
{
    //initialize tracking variables
    Queue<Flight> unassignedFlights = new Queue<Flight>();
    int totalUnassigned = 0;
    int totalUnassignedGates = 0;

    //identify unassigned flights
    foreach (var flight in terminal.Flights.Values)
    {
        bool isAssigned = false;
        foreach (var gate in terminal.BoardingGates.Values)
        {
            if (gate.Flight?.FlightNumber == flight.FlightNumber)
            {
                isAssigned = true;
                break;
            }
        }
        if (!isAssigned)
        {
            unassignedFlights.Enqueue(flight);
            totalUnassigned++;
        }
    }
    Console.WriteLine($"Total unassigned flights: {totalUnassigned}");

    //find available gates
    var availableGates = terminal.BoardingGates.Values
        .Where(g => g.Flight == null)
        .ToList();
    totalUnassignedGates = availableGates.Count;
    Console.WriteLine($"Total available gates: {totalUnassignedGates}");

    //exit if no work needed
    if (totalUnassigned == 0)
    {
        Console.WriteLine("No unassigned flights to process.");
        return;
    }

    //process queue
    int processedCount = 0;
    int automaticAssignments = 0;

    while (unassignedFlights.Count > 0 && availableGates.Count > 0)
    {
        var flight = unassignedFlights.Dequeue();
        BoardingGate? assignedGate = null;

        //match gate based on flight special requirements
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

        //process successful assignments
        if (assignedGate != null)
        {
            //make assignment
            assignedGate.Flight = flight;
            availableGates.Remove(assignedGate);
            automaticAssignments++;

            //display assignment details
            string airlineCode = flight.FlightNumber.Split(' ')[0];
            string airlineName = terminal.Airlines.ContainsKey(airlineCode)
                ? terminal.Airlines[airlineCode].Name
                : "Unknown";

            Console.WriteLine("\nAssigned Flight:");
            Console.WriteLine($"{"Flight No",-12} {"Airline",-20} {"Origin",-20} " +
                            $"{"Destination",-20} {"Expected Time",-25} " +
                            $"{"Special Request",-15} {"Assigned Gate"}");

            string specialRequest = flight switch
            {
                DDJBFlight => "DDJB",
                CFFTFlight => "CFFT",
                LWTTFlight => "LWTT",
                _ => "None"
            };

            Console.WriteLine($"{flight.FlightNumber,-12} {airlineName,-20} " +
                            $"{flight.Origin,-20} {flight.Destination,-20} " +
                            $"{flight.ExpectedTime:dd/M/yyyy h:mm:ss tt} " +
                            $"{specialRequest,-15} {assignedGate.GateName}");
        }

        processedCount++;
    }

    //display summary statistics
    Console.WriteLine("\nAssignment Summary:");
    Console.WriteLine($"Total flights processed: {processedCount}");
    Console.WriteLine($"Successful automatic assignments: {automaticAssignments}");

    double automaticPercentage = (double)automaticAssignments / totalUnassigned * 100;
    Console.WriteLine($"Automatic assignment percentage: {automaticPercentage:F1}%");

    if (unassignedFlights.Count > 0)
    {
        Console.WriteLine($"\nWarning: {unassignedFlights.Count} flights remain unassigned " +
                         "due to insufficient matching gates.");
    }
}

//Advanced Feature (b)
bool ValidateFlightAssignments(Terminal terminal)
{
    //iterate through airlines and flights to ensure all flights are assigned to boarding gates
    foreach (var airline in terminal.Airlines.Values)
    {
        foreach (var flight in airline.Flights.Values)
        {
            //check if flight is assigned to a boarding gate
            bool isAssigned = false;
            foreach (var bGate in terminal.BoardingGates.Values)
            {
                //check if flight is assigned to a boarding gate
                if (bGate.Flight == flight)
                {
                    isAssigned = true;
                    break;
                }
            }
            //display warning message if flight is not assigned to any boarding gate
            if (!isAssigned)
            {
                Console.WriteLine($"Flight {flight.FlightNumber} is not assigned to any boarding gate!");
                return false;
            }
        }
    }
    //return true if all flights are assigned to boarding gates
    return true;
}
//method to calculate total fees per airline for the day
void CalculateDailyFees(Terminal terminal)
{
    try
    {
        //validate flight assignments
        if (!ValidateFlightAssignments(terminal))
        {
            Console.WriteLine("Cannot calculate fees. Please ensure all flights are assigned to boarding gates.");
            return;
        }
        terminal.PrintAirlineFees();
    }
    //general exception handling
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
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
    Console.WriteLine("8. Process all unassigned flights to boarding gates in bulk");
    Console.WriteLine("9. Display the total fee per airline for the day");
    Console.WriteLine("0. Exit");
    Console.WriteLine("");
}

//main
Terminal terminal = new Terminal("Terminal 5");
CreateAirlines(terminal);
CreateBoardingGates(terminal);
CreateFlights(terminal);
Console.WriteLine("Loading Airlines...");
Console.WriteLine($"{terminal.Airlines.Count} Airlines Loaded!");
Console.WriteLine($"Loading Boarding Gates...");
Console.WriteLine($"{terminal.BoardingGates.Count} Boarding Gates Loaded!");
Console.WriteLine($"Loading Flights...");
Console.WriteLine($"{terminal.Flights.Count} Flights Loaded!");

while (true)
{
    DisplayMenu();
    Console.WriteLine("Please select your option:");
    string? option = Console.ReadLine();
    if (!int.TryParse(option, out int validOption) || validOption < 0 || validOption > 9)
    {
        Console.WriteLine("Invalid option! Please try again.");
        continue;
    }
    try
    {
        if (option == "0")
        {
            Console.WriteLine("Goodbye!");
            break;
        }
        else if (option == "1")
        {
            ListAllFlights(terminal);  //Feature 3
        }
        else if (option == "2")
        {
            DisplayAllBoardingGates(terminal); //Feature 4
        }
        else if (option == "3")
        {
            AssignBoardingGate(terminal);
        }
        else if (option == "4")
        {
            CreateNewFlight(terminal);  //Feature 6
        }
        else if (option == "5")
        {
            HandleDisplayAirlineFlights(terminal); //Feature 7
        }
        else if (option == "6")
        {
            HandleModifyOrDeleteFlight(terminal); //Feature 8
        }
        else if (option == "7")
        {
            DisplayFlightSchedule(terminal);  //Feature 9
        }
        else if (option == "8")
        {
            ProcessUnassignedFlights(terminal);  //Advanced Feature (a)
        }
        else if (option == "9")
        {
            CalculateDailyFees(terminal); //Advanced Feature (b)
        }
        else
        {
            Console.WriteLine("Invalid option! Please try again.");
        }
    }
    catch (FormatException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}