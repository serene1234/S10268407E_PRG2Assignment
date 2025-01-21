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
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }
        public Dictionary<string, BoardingGate> BoardingGates { get; set; }
        public Dictionary<string, double> GateFees { get; set; }
        public Terminal(string tName)
        {
            TerminalName = tName;
            Airlines = new Dictionary<string, Airline>();
            Flights = new Dictionary<string, Flight>();
            BoardingGates = new Dictionary<string, BoardingGate>();
            GateFees = new Dictionary<string, double>();
        }
        public bool AddAirLine(Airline airline)
        {
            if (Airlines.ContainsKey(airline.Name))
            {
                return false;
            }
            else
            {
                Airlines.Add(airline.Name, airline);
                return true;
            }
        }
        public bool AddBoardingGate(BoardingGate boardingGate)
        {
            if (BoardingGates.ContainsKey(boardingGate.GateName))
            {
                return false;
            }
            else
            {
                BoardingGates.Add(boardingGate.GateName, boardingGate);
                return true;
            }
        }
        public Airline GetAirlineFromFlight(Flight flight)
        {
            foreach (var airline in Airlines.Values)
            {
                if (airline.Flights.ContainsValue(flight))
                {
                    return airline;
                }
            }
            return null;
        }
        public void PrintAirlineFees()
        {
            foreach (var airline in Airlines.Values)
            {
                Console.WriteLine($"Airline: {airline.Name}\tFee: {airline.CalculateFees()}");
            }
        }
        public override string ToString()
        {
            return "Terminal Name: " + TerminalName + "\tNumber of Airlines: " + Airlines.Count + "\tNumber of Flights: " + Flights.Count + "\tNumber of Boarding Gates: " + BoardingGates.Count;
        }
    }
}
