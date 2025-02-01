//==========================================================
// Student Number	: S10268407E
// Student Name	: Serene Ker Xin Yun
// Partner Name	: Shawntrice Yip Yin Fei
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10268407E_PRG2Assignment
{
    class Terminal
    {
        //properties
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }
        public Dictionary<string, BoardingGate> BoardingGates { get; set; }
        public Dictionary<string, double> GateFees { get; set; }
        //constructor
        public Terminal(string tName)
        {
            TerminalName = tName;
            Airlines = new Dictionary<string, Airline>();
            Flights = new Dictionary<string, Flight>();
            BoardingGates = new Dictionary<string, BoardingGate>();
            GateFees = new Dictionary<string, double>();
        }
        //methods
        public bool AddAirline(Airline airline)
        {
            //check if airline code already exists
            if (Airlines.ContainsKey(airline.Code))
            {
                return false;
            }
            //add airline if code does not exist
            else
            {
                Airlines.Add(airline.Code, airline);
                return true;
            }
        }
        public bool AddBoardingGate(BoardingGate boardingGate)
        {
            //check if boarding gate name already exists
            if (BoardingGates.ContainsKey(boardingGate.GateName))
            {
                return false;
            }
            //add boarding gate if name does not exist
            else
            {
                BoardingGates.Add(boardingGate.GateName, boardingGate);
                return true;
            }
        }
        public Airline GetAirlineFromFlight(Flight flight)
        {
            //iterate through airlines to find airline with flight
            foreach (var airline in Airlines.Values)
            {
                //check if airline contains flight
                if (airline.Flights.ContainsValue(flight))
                {
                    //return airline with flight
                    return airline;
                }
            }
            //return null if airline with flight is not found
            return null;
        }
        public void PrintAirlineFees()
        {
            //initialize total terminal fees and discounts
            double totalTerminalFees = 0;
            double totalTerminalDiscounts = 0;
            //iterate through airlines to calculate fees and discounts
            foreach (var airline in Airlines.Values)
            {
                //initialize airline subtotal and discounts
                double airlineSubtotal = airline.CalculateFees();
                double airlineDiscounts = 0;
                //count number of flights for airline
                int flightCount = airline.Flights.Count;
                //iterate through flights to calculate fees and discounts
                foreach (var flight in airline.Flights.Values)
                {
                    //check if flight is scheduled for the current date
                    if (flight.ExpectedTime.Date != DateTime.Today)
                    {
                        //skip flights scheduled for other dates
                        continue;
                    }
                    //initialize eligible flight discounts
                    double eligibleFlightDiscounts = 0.0;
                    foreach (var bGate in BoardingGates.Values)
                    {
                        if (bGate.Flight == flight)
                        {
                            airlineSubtotal += bGate.CalculateFees();
                            break;
                        }
                    }
                    //apply discounts for origin
                    if (flight.Origin == "Dubai (DXB)" || flight.Origin == "Bangkok (BKK)" || flight.Origin == "Tokyo (NRT)")
                    {
                        eligibleFlightDiscounts += 25.0;
                    }
                    //apply discounts for no special request
                    if (flight is NORMFlight)
                    {
                        eligibleFlightDiscounts += 50.0;
                    }
                    //apply discounts for expected time within discounted range
                    if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour > 21)
                    {
                        eligibleFlightDiscounts += 110.0;
                    }
                    //add eligible flight discounts to airline's total discounts
                    airlineDiscounts += eligibleFlightDiscounts;
                }
                //apply discounts for every 3 flights
                airlineDiscounts += Math.Floor(flightCount / 3.0) * 350.0;
                //apply discounts for airlines with more than 5 flights
                if (flightCount > 5)
                {
                    airlineDiscounts += airlineSubtotal * 0.03;
                }
                //calculate final total fees for the airline after disocunts
                double airlineFinalTotal = airlineSubtotal - airlineDiscounts;
                //update total terminal fees and discounts
                totalTerminalFees += airlineSubtotal;
                totalTerminalDiscounts += airlineDiscounts;
                //display fees and discounts per airline
                Console.WriteLine($"Airline: {airline.Name}");
                Console.WriteLine($"Subtotal: {airlineSubtotal:C2}");
                Console.WriteLine($"Discounts Applied: -{airlineDiscounts:C2}");
                Console.WriteLine($"Final Amount Charged: {airlineFinalTotal:C2}");
                Console.WriteLine();
            }
            //display total Terminal 5 revenue and discounts
            Console.WriteLine("========== Terminal 5 Summary ==========");
            Console.WriteLine($"Total Fees Before Discounts: {totalTerminalFees:C2}");
            Console.WriteLine($"Total Discounts Applied: -{totalTerminalDiscounts:C2}");
            Console.WriteLine($"Final Fees Terminal 5 Will Collect: {totalTerminalFees - totalTerminalDiscounts:C2}");
            Console.WriteLine($"Discount Percentage: {((totalTerminalDiscounts / (totalTerminalFees - totalTerminalDiscounts)) * 100):F2}%");
        }
        public override string ToString()
        {
            return "Terminal Name: " + TerminalName + "\tNumber of Airlines: " + Airlines.Count + "\tNumber of Flights: " + Flights.Count + "\tNumber of Boarding Gates: " + BoardingGates.Count;
        }
    }
}
