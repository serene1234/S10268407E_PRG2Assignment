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
    class Flight : IComparable<Flight>
    {
        //properties
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }

        //constructor
        public Flight(string flightNum, string origin, string dest, DateTime expectedTime)
        {
            FlightNumber = flightNum;
            Origin = origin;
            Destination = dest;
            ExpectedTime = expectedTime;
            Status = "Scheduled"; //default status
        }

        //calculate fees based on flight type
        public virtual double CalculateFees()
        {
            double fees = 0; //base fee of 300
            //fees calculation
            if (Origin == "Singapore (SIN)")
                fees = 800; //departing flight fee
            else if (Destination == "Singapore (SIN)")
                fees = 500; //arriving flight fee
            return fees;
        }

        //Feature 9
        public int CompareTo(Flight other)
        {
            if (other == null) return 1;
            return ExpectedTime.CompareTo(other.ExpectedTime);
        }

        //override ToString for display
        public override string ToString()
        {
            return $"{FlightNumber,-12} {Origin,-20} {Destination,-20} {ExpectedTime.ToString("dd/M/yyyy h:mm:ss tt"),-25} {Status,-15}";
        }
    }
}