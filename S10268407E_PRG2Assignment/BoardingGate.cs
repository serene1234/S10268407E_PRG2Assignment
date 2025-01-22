﻿//==========================================================
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
        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }
        public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
        {
            GateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportsDDJB;
            SupportsLWTT = supportsLWTT;
            Flight = null;
        }
        public double CalculateFees() 
        {
            double baseFee = 300.0;
            if (SupportsCFFT)
            {
                baseFee += 150.0;
            }
            else if (SupportsDDJB)
            {
                baseFee += 300.0;
            }
            else if (SupportsLWTT)
            {
                baseFee += 500.0;
            }
            return baseFee;
        }
        public override string ToString()
        {
            return "Gate Name: " + GateName + "\tSupports CFFT: " + SupportsCFFT + "\tSupports DDJB: " + SupportsDDJB + "\tSupports LWTT: " + SupportsLWTT;
        }
    }
}
