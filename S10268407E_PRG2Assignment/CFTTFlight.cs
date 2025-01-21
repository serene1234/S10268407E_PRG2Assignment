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
    public class CFTTFlight : Flight
    {
        //properties
        public double RequestFee { get; set; } = 150; //CFFT specific request fee

        //constructor
        public CFTTFlight(string flightNum, string origin, string dest, DateTime time)
            : base(flightNum, origin, dest, time) { }

        //override CalculateFees to include CFFT specific charges
        public override double CalculateFees()
        {
            double baseFees = base.CalculateFees();
            return baseFees + RequestFee;
        }

        //override ToString to include CFFT specific information
        public override string ToString()
        {
            return base.ToString() + $"\nSpecial Request Code: CFFT";
        }
    }
}