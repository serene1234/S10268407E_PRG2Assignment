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
    class CFFTFlight : Flight
    {
        //properties
        public double RequestFee { get; private set; }

        //constructor
        public CFFTFlight(string flightNum, string origin, string dest, DateTime expectedTime)
            : base(flightNum, origin, dest, expectedTime)
        {
            RequestFee = 150.0; //CFFT code request fee
        }

        //calculate fees including CFFT fee
        public override double CalculateFees()
        {
            return base.CalculateFees() + RequestFee;
        }

        //override ToString for display
        public override string ToString()
        {
            return base.ToString() + " [CFFT]";
        }
    }
}