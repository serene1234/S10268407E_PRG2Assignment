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
    class NORMFlight : Flight
    {
        //constructor
        public NORMFlight(string flightNum, string origin, string dest, DateTime expectedTime)
            : base(flightNum, origin, dest, expectedTime) { }

        //calculate fees for normal flights
        public override double CalculateFees()
        {
            return base.CalculateFees(); //only base fees apply
        }

        //override ToString for display
        public override string ToString()
        {
            return base.ToString();
        }
    }
}