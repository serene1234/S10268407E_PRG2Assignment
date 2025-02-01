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
    class BoardingGate
    {
        //properties
        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }
        //constructor
        public BoardingGate() { }
        public BoardingGate(string gateName, bool supportsDDJB, bool supportsCFFT, bool supportsLWTT)
        {
            GateName = gateName;
            SupportsDDJB = supportsDDJB;
            SupportsCFFT = supportsCFFT;
            SupportsLWTT = supportsLWTT;
        }
        //methods
        public double CalculateFees() 
        {
            //base fee of 300
            return 300;
        }
        public override string ToString()
        {
            return ($"{GateName,-16}{SupportsDDJB,-23}{SupportsCFFT,-23}{SupportsLWTT,-23}");
        }
    }
}
