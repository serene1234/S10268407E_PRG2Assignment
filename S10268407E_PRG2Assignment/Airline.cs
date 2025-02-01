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
        //properties
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }
        //constructor
        public Airline() { }
        public Airline(string name, string code)
        {
            Name = name;
            Code = code;
            Flights = new Dictionary<string, Flight>();
        }
        //methods
        public bool AddFlight(Flight flight)
        {
            //check if flight number already exists
            if (!Flights.ContainsKey(flight.FlightNumber))
            {
                //add flight if number does not exist
                Flights.Add(flight.FlightNumber, flight);
                return true;
            }
            //return false if flight number already exists
            return false;
        }
        //calculate total fees of all flights
        public double CalculateFees()
        {
            //initialize total fees
            double totalFees = 0;
            foreach (var flight in Flights.Values)
            {
                if (flight is NORMFlight)
                {
                    //calculate fees for NORM flight
                    totalFees += flight.CalculateFees();
                }
                else if  (flight is CFFTFlight)
                {
                    //calculate fees for CFFT flight
                    CFFTFlight cFFTFlight = (CFFTFlight)flight;
                    totalFees += cFFTFlight.CalculateFees();
                }
                else if (flight is DDJBFlight)
                {
                    //calculate fees for DDJB flight
                    DDJBFlight dDJBFlight = (DDJBFlight)flight;
                    totalFees += dDJBFlight.CalculateFees();
                }
                else if (flight is LWTTFlight)
                {
                    //calculate fees for LWTT flight
                    LWTTFlight lWTTFlight = (LWTTFlight)flight;
                    totalFees += lWTTFlight.CalculateFees();
                }
            }
            //return total fees
            return totalFees;
        }
        //remove flight from airline
        public bool RemoveFlight(Flight flight)
        {
            //check if flight number exists
            if (Flights.ContainsKey(flight.FlightNumber))
            {
                //remove flight if number exists
                Flights.Remove(flight.FlightNumber);
                return true;
            }
            //return false if flight number does not exist
            return false;
        }
        public override string ToString()
        {
            return ($"{Code,-16}{Name,-18}");
        }
    }
}
