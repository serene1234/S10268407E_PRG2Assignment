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
    public class LWTTFlight : Flight
    {
        //properties
        public double RequestFee { get; set; } = 500; //LWTT specific request fee

        //constructor
        public LWTTFlight(string flightNum, string origin, string dest, DateTime time)
            : base(flightNum, origin, dest, time) { }

        //override CalculateFees to include LWTT specific charges
        public override double CalculateFees()
        {
            double baseFees = base.CalculateFees();
            return baseFees + RequestFee;
        }

        //override ToString to include LWTT specific information
        public override string ToString()
        {
            return base.ToString() + $"\nSpecial Request Code: LWTT";
        }
    }
}