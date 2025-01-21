//==========================================================
// Student Number	: S10270521C
// Student Name	: Shawntrice Yip Yin Fei
// Partner Name	: Serene Ker Xin Yun
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10268407E_PRG2Assignment
{
    public class Flight : IComparable<Flight>
    {
        // Properties
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; } = "Scheduled"; // Default status

        // Constructor
        public Flight() { }

        public Flight(string flightNum, string origin, string dest, DateTime time)
        {
            FlightNumber = flightNum;
            Origin = origin;
            Destination = dest;
            ExpectedTime = time;
        }

        // Methods
        public virtual double CalculateFees()
        {
            // Base calculation logic
            double fees = 0;

            // Check if arriving or departing from Singapore
            if (Destination.Contains("SIN"))
                fees += 500; // Arriving flight fee
            if (Origin.Contains("SIN"))
                fees += 800; // Departing flight fee

            return fees;
        }

        // Required for IComparable implementation - Used in Feature 9
        public int CompareTo(Flight other)
        {
            if (other == null) return 1;
            return ExpectedTime.CompareTo(other.ExpectedTime);
        }

        // ToString override for displaying flight information
        public override string ToString()
        {
            return $"Flight Number: {FlightNumber}\n" +
                   $"Origin: {Origin}\n" +
                   $"Destination: {Destination}\n" +
                   $"Expected Time: {ExpectedTime:dd/M/yyyy h:mm:ss tt}\n" +
                   $"Status: {Status}";
        }
    }
}