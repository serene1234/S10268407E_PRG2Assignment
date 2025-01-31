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
    class Airline
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }
        public Airline() { }
        public Airline(string name, string code)
        {
            Name = name;
            Code = code;
            Flights = new Dictionary<string, Flight>();
        }
        public bool AddFlight(Flight flight)
        {
            if (!Flights.ContainsKey(flight.FlightNumber))
            {
                Flights.Add(flight.FlightNumber, flight);
                return true;
            }
            return false;
        }
        public double CalculateFees()
        {
            double totalFees = 0;
            foreach (var flight in Flights.Values)
            {
                if (flight is NORMFlight)
                {
                    totalFees += flight.CalculateFees();
                }
                else if  (flight is CFFTFlight)
                {
                    CFFTFlight cFFTFlight = (CFFTFlight)flight;
                    totalFees += cFFTFlight.CalculateFees();
                }
                else if (flight is DDJBFlight)
                {
                    DDJBFlight dDJBFlight = (DDJBFlight)flight;
                    totalFees += dDJBFlight.CalculateFees();
                }
                else if (flight is LWTTFlight)
                {
                    LWTTFlight lWTTFlight = (LWTTFlight)flight;
                    totalFees += lWTTFlight.CalculateFees();
                }
            }
            return totalFees;
        }
        public bool RemoveFlight(Flight flight)
        {
            if (Flights.ContainsKey(flight.FlightNumber))
            {
                Flights.Remove(flight.FlightNumber);
                return true;
            }
            return false;
        }
        public override string ToString()
        {
            return ($"{Code,-16}{Name,-18}");
        }
    }
}
